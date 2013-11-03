using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trailers.Extensions;
using Trailers.Providers.TMDb.DataStructures;
using Trailers.Web;

namespace Trailers.Providers.TMDb.API
{
    public static class TMDbAPI
    {
        #region API variables

        const string apiKey = "e636af47bb9604b7fe591847a98ca408";
        private const string apiUrl = "http://api.themoviedb.org/3/";
        
        private static string apiMovieTrailers = string.Concat(apiUrl, "movie/{0}/trailers?api_key=", apiKey);

        #endregion

        #region Public Methods

        public static TMDbTrailers GetMovieTrailers(string movieId)
        {
            string response = GetJson(string.Format(apiMovieTrailers, movieId));
            return response.FromJson<TMDbTrailers>();
        }

        #endregion

        #region Private Methods

        private static string GetJson(string url)
        {
            WebGrabber grabber = WebUtils.GetWebGrabberInstance(url);
            grabber.Encoding = Encoding.UTF8;
            grabber.Accept = "application/json";

            if (grabber.GetResponse())
                return grabber.GetString();
            else
                return null;
        }

        #endregion
    }
}
