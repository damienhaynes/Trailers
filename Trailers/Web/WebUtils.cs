using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Trailers.Web
{
    enum DownloadStatus
    {
        SUCCESS, INCOMPLETE, TIMED_OUT, FAILED
    }

    class WebUtils
    {
        public static WebGrabber GetWebGrabberInstance(string url)
        {
            WebGrabber grabber = new WebGrabber(url);
            grabber.UserAgent = PluginSettings.UserAgent;
            grabber.MaxRetries = PluginSettings.WebMaxRetries;
            grabber.Timeout = PluginSettings.WebTimeout;
            grabber.TimeoutIncrement = PluginSettings.WebTimeoutIncrement;
            return grabber;
        }

        public static string GetYouTubeURL(string key)
        {
            // check it's not already a youtube url
            if (key.ToLowerInvariant().Contains("youtube.com")) 
                return key;

            // check if it start's with 'v=' and remove it
            if (key.StartsWith("v="))
                key = key.Replace("v=", string.Empty);
            
            // remove hd option as that is not needed
            // it's worked out based on OnlineVideo settings
            if (key.EndsWith("&hd=1"))
                key = key.Replace("&hd=1", string.Empty);

            return string.Format("http://www.youtube.com/watch?v={0}", key);
        }

        public static bool DownloadFile(string url, string filename)
        {
            FileLog.Debug("Initiating trailer download for prefered download option");

            if (File.Exists(filename))
            {
                FileLog.Info("Trailer '{0}' already exists on disk, aborting download", filename);
                return true;
            }

            int maxRetries = PluginSettings.WebMaxRetries;
            int timeoutBase = PluginSettings.WebTimeout;
            int timeoutIncrement = PluginSettings.WebTimeoutIncrement;

            DownloadStatus status = DownloadStatus.INCOMPLETE;
            long position = 0;
            int resumeAttempts = 0;
            int retryAttempts = 0;

            while (status != DownloadStatus.SUCCESS && status != DownloadStatus.FAILED)
            {
                int timeout = timeoutBase + (timeoutIncrement * retryAttempts);

                status = Download(url, position, timeout, filename);

                switch (status)
                {
                    case DownloadStatus.INCOMPLETE:
                        // if the download ended half way through, log a warning and update the 
                        // position for a resume
                        if (File.Exists(filename)) position = new FileInfo(filename).Length;
                        resumeAttempts++;
                        if (resumeAttempts > 1)
                        {
                            FileLog.Warning("Connection lost while downloading trailer. Attempting to resume...");
                        }
                        break;

                    case DownloadStatus.TIMED_OUT:
                        // if we timed out past our try limit, fail
                        retryAttempts++;
                        if (retryAttempts == maxRetries)
                        {
                            FileLog.Error("Failed downloading trailer from: '{0}'. Reached retry limit of '{1}'", url, maxRetries);
                            status = DownloadStatus.FAILED;
                        }
                        break;
                }
            }

            if (status == DownloadStatus.SUCCESS)
            {
                FileLog.Info("Successfully downloaded trailer to: '{0}'", filename);
                return true;
            }
            else
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                return false;
            }
        }

        static DownloadStatus Download(string url, long startPosition, int timeout, string filename)
        {
            DownloadStatus rtn = DownloadStatus.SUCCESS;

            HttpWebRequest request = null;
            WebResponse response = null;
            Stream webStream = null;
            FileStream fileStream = null;

            try
            {
                // setup our file to be written to
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));

                if (startPosition == 0)
                    fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                else
                    fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None);

                // setup and open our connection
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AddRange((int)startPosition);
                request.Timeout = timeout;
                request.ReadWriteTimeout = 20000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:10.0) Gecko/20100101 Firefox/26.0";
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                response = request.GetResponse();
                webStream = response.GetResponseStream();

                // setup our tracking variables for progress
                int bytesRead = 0;
                long totalBytesRead = 0;
                long totalBytes = response.ContentLength + startPosition;

                // download the file and progressively write it to disk
                byte[] buffer = new byte[2048];
                bytesRead = webStream.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    // write to our file
                    fileStream.Write(buffer, 0, bytesRead);
                    totalBytesRead = fileStream.Length;

                    //FileLog.Debug("Download progress: {2:0.0}% ({0:###,###,###} / {1:###,###,###} bytes)", totalBytesRead, totalBytes, 100.0 * totalBytesRead / totalBytes);

                    // read the next stretch of data
                    bytesRead = webStream.Read(buffer, 0, buffer.Length);
                }

                // if the downloaded unexpectedly stopped, but we have more left, close the stream but 
                // save the file for resuming
                if (fileStream.Length != totalBytes && totalBytes != -1)
                {
                    rtn = DownloadStatus.INCOMPLETE;
                    fileStream.Close();
                    fileStream = null;
                }
                // if the download stopped and we don't know the total size
                else if (totalBytes == -1)
                {
                    try
                    {
                        fileStream.Close();
                        // we could a check if a valid mp4 file here and return success if okay
                    }
                    catch
                    {
                        startPosition = 0;
                        FileLog.Warning("Invalid trailer when downloading file from: " + url);
                        rtn = DownloadStatus.FAILED;
                    }
                }
            }
            catch (UriFormatException)
            {
                // url was invalid
                FileLog.Warning("Invalid URL: {0}", url);
                rtn = DownloadStatus.FAILED;
            }
            catch (WebException e)
            {
                // file doesnt exist
                if (e.Message.Contains("404"))
                {
                    FileLog.Warning("File does not exist: {0}", url);
                    rtn = DownloadStatus.FAILED;
                }
                // timed out or other similar error
                else
                    rtn = DownloadStatus.TIMED_OUT;

            }
            catch (ThreadAbortException)
            {
                // user is shutting down the program
                fileStream.Close();
                fileStream = null;
                if (File.Exists(filename)) File.Delete(filename);
                rtn = DownloadStatus.FAILED;
            }
            catch (Exception e)
            {
                FileLog.Error("Unexpected error downloading file from: {0}, {1}", url, e.Message);
                rtn = DownloadStatus.FAILED;
            }

            // if we failed delete the file
            if (rtn == DownloadStatus.FAILED)
            {
                if (fileStream != null) fileStream.Close();
                fileStream = null;
                if (File.Exists(filename)) File.Delete(filename);
            }

            if (webStream != null) webStream.Close();
            if (fileStream != null) fileStream.Close();
            if (response != null) response.Close();
            if (request != null) request.Abort();

            return rtn;
        }
    }
}
