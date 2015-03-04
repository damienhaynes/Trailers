using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Trailers.Extensions;
using Trailers.Providers.TMDb.DataStructures;
using Trailers.Web;

namespace Trailers.Providers.TMDb.API
{
    public static class TMDbAPI
    {
        #region Enums
        public enum ExternalSource
        {
            imdb_id,
            tvrage_id,
            tvdb_id
        }
        #endregion

        #region API variables

        const string apiKey = "e636af47bb9604b7fe591847a98ca408";

        // base API url
        private const string apiUrl = "http://api.themoviedb.org/3/";
        
        // trailer uri's
        private static string apiMovieTrailers = string.Concat(apiUrl, "movie/{0}/videos?language={1}&api_key=", apiKey);
        private static string apiShowTrailers = string.Concat(apiUrl, "tv/{0}/videos?language={1}&api_key=", apiKey);
        private static string apiSeasonTrailers = string.Concat(apiUrl, "tv/{0}/season/{1}/videos?language={2}&api_key=", apiKey);
        private static string apiEpisodeTrailers = string.Concat(apiUrl, "tv/{0}/season/{1}/episode/{2}/videos?language={3}&api_key=", apiKey);

        // search uri's
        private static string apiMovieSearch = string.Concat(apiUrl, "search/movie?api_key=", apiKey, "&query={0}&page={1}&language={2}&include_adult={3}&year={4}");
        private static string apiShowSearch = string.Concat(apiUrl, "search/tv?api_key=", apiKey, "&query={0}&page={1}&language={2}&first_air_date_year={3}");
        private static string apiFind = string.Concat(apiUrl, "find/{0}?&external_source={1}&api_key=", apiKey);

        private static string apiEpisodeExIds = string.Concat(apiUrl, "tv/{0}/season/{1}/episode/{2}/external_ids?language={3}&api_key=", apiKey);
        #endregion

        #region Events
        // these events can be used to log data sent / received from themoviedb.org
        internal delegate void OnDataSendDelegate(string url, string postData);
        internal delegate void OnDataReceivedDelegate(string response);

        internal static event OnDataSendDelegate OnDataSend;
        internal static event OnDataReceivedDelegate OnDataReceived;
        #endregion

        #region Public Methods

        public static TMDbTrailers GetMovieTrailers(string movieId, string language = "en", bool fallbackToEnglish = false, bool addEnglish = false)
        {
            string response = GetJson(string.Format(apiMovieTrailers, movieId, language));
            var trailers = response.FromJson<TMDbTrailers>();

            if (trailers == null || trailers.Results == null)
                return null;

            if (!fallbackToEnglish && !addEnglish) 
                return trailers;

            if (language != "en")
            {
                // if no results get english trailers
                if (trailers.Results.Count == 0 && fallbackToEnglish)
                {
                    response = GetJson(string.Format(apiMovieTrailers, movieId, "en"));
                    return response.FromJson<TMDbTrailers>();
                }

                // also get the english trailers
                if (addEnglish)
                {
                    response = GetJson(string.Format(apiMovieTrailers, movieId, "en"));
                    var englishTrailers = response.FromJson<TMDbTrailers>();
                    if (englishTrailers == null || englishTrailers.Results == null)
                        return trailers;

                    trailers.Results = trailers.Results.Union(englishTrailers.Results).ToList();
                    return trailers;
                }
            }

            return trailers;
        }

        public static TMDbTrailers GetShowTrailers(string showid, string language = "en", bool fallbackToEnglish = false, bool addEnglish = false)
        {
            string response = GetJson(string.Format(apiShowTrailers, showid, language));
            var trailers = response.FromJson<TMDbTrailers>();

            if (trailers == null || trailers.Results == null)
                return null;

            if (!fallbackToEnglish && !addEnglish)
                return trailers;

            if (language != "en")
            {
                // if no results get english trailers
                if (trailers.Results.Count == 0 && fallbackToEnglish)
                {
                    response = GetJson(string.Format(apiShowTrailers, showid, "en"));
                    return response.FromJson<TMDbTrailers>();
                }

                // also get the english trailers
                if (addEnglish)
                {
                    response = GetJson(string.Format(apiShowTrailers, showid, "en"));
                    var englishTrailers = response.FromJson<TMDbTrailers>();
                    if (englishTrailers == null || englishTrailers.Results == null)
                        return trailers;

                    trailers.Results = trailers.Results.Union(englishTrailers.Results).ToList();
                    return trailers;
                }
            }

            return trailers;
        }

        public static TMDbTrailers GetSeasonTrailers(string showid, string season, string language = "en", bool fallbackToEnglish = false, bool addEnglish = false)
        {
            string response = GetJson(string.Format(apiSeasonTrailers, showid, season, language));
            var trailers = response.FromJson<TMDbTrailers>();

            if (trailers == null || trailers.Results == null)
                return null;

            if (!fallbackToEnglish && !addEnglish)
                return trailers;

            if (language != "en")
            {
                // if no results get english trailers
                if (trailers.Results.Count == 0 && fallbackToEnglish)
                {
                    response = GetJson(string.Format(apiSeasonTrailers, showid, season, "en"));
                    return response.FromJson<TMDbTrailers>();
                }

                // also get the english trailers
                if (addEnglish)
                {
                    response = GetJson(string.Format(apiSeasonTrailers, showid, season, "en"));
                    var englishTrailers = response.FromJson<TMDbTrailers>();
                    if (englishTrailers == null || englishTrailers.Results == null)
                        return trailers;

                    trailers.Results = trailers.Results.Union(englishTrailers.Results).ToList();
                    return trailers;
                }
            }

            return trailers;
        }

        public static TMDbTrailers GetEpisodeTrailers(string showid, string season, string episode, string language = "en", bool fallbackToEnglish = false, bool addEnglish = false)
        {
            string response = GetJson(string.Format(apiEpisodeTrailers, showid, season, episode, language));
            var trailers = response.FromJson<TMDbTrailers>();

            if (trailers == null || trailers.Results == null)
                return null;

            if (!fallbackToEnglish && !addEnglish)
                return trailers;

            if (language != "en")
            {
                // if no results get english trailers
                if (trailers.Results.Count == 0 && fallbackToEnglish)
                {
                    response = GetJson(string.Format(apiEpisodeTrailers, showid, season, episode, "en"));
                    return response.FromJson<TMDbTrailers>();
                }

                // also get the english trailers
                if (addEnglish)
                {
                    response = GetJson(string.Format(apiEpisodeTrailers, showid, season, episode, "en"));
                    var englishTrailers = response.FromJson<TMDbTrailers>();
                    if (englishTrailers == null || englishTrailers.Results == null)
                        return trailers;

                    trailers.Results = trailers.Results.Union(englishTrailers.Results).ToList();
                    return trailers;
                }
            }

            return trailers;
        }

        public static TMDbMovieSearch SearchMovies(string searchStr, int page = 1, string language = "en", bool includeAdult = false, string year = null)
        {
            string response = GetJson(string.Format(apiMovieSearch, HttpUtility.UrlEncode(searchStr), page.ToString(), language, includeAdult.ToString(), year ?? string.Empty));
            return response.FromJson<TMDbMovieSearch>();
        }

        public static TMDbShowSearch SearchShows(string searchStr, int page = 1, string language = "en", string firstAirDate = null)
        {
            string response = GetJson(string.Format(apiShowSearch, HttpUtility.UrlEncode(searchStr), page.ToString(), language, firstAirDate ?? string.Empty));
            return response.FromJson<TMDbShowSearch>();
        }

        public static TMDbFindResult TMDbFind(string id, ExternalSource sourceId)
        {
            string response = GetJson(string.Format(apiFind, id, sourceId.ToString()));
            return response.FromJson<TMDbFindResult>();
        }

        public static TMDbExternalId GetEpisodeExternalIds(int showid, int season, int episode, string language = "en")
        {
            var response = GetJson(string.Format(apiEpisodeExIds, showid, season, episode, "en"));
            FileLog.Info(response);
            return response.FromJson<TMDbExternalId>();
        }

        #endregion        

        #region Private Methods

        private static string GetJson(string url, string postData = null)
        {
            if (OnDataSend != null)
                OnDataSend(url, postData);

            WebGrabber grabber = WebUtils.GetWebGrabberInstance(url);
            grabber.Encoding = Encoding.UTF8;
            grabber.Accept = "application/json";

            if (grabber.GetResponse())
            {
                string response = grabber.GetString();
                
                if (OnDataReceived != null) 
                    OnDataReceived(response);

                return response;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
