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
        #region API variables

        const string apiKey = "e636af47bb9604b7fe591847a98ca408";
        private const string apiUrl = "http://api.themoviedb.org/3/";
        
        private static string apiMovieTrailers = string.Concat(apiUrl, "movie/{0}/trailers?api_key=", apiKey);
        private static string apiMovieSearch = string.Concat(apiUrl, "search/movie?api_key=", apiKey, "&query={0}&page={1}&language={2}&include_adult={3}&year={4}");

        #endregion

        #region Public Methods

        public static TMDbTrailers GetMovieTrailers(string movieId)
        {
            string response = GetJson(string.Format(apiMovieTrailers, movieId));
            return response.FromJson<TMDbTrailers>();
        }

        public static TMDbMovieSearch SearchMovies(string searchStr, int page = 1, string language = "en", bool includeAdult = false, string year = null)
        {
            string response = GetJson(string.Format(apiMovieSearch, HttpUtility.UrlEncode(searchStr), page.ToString(), language, includeAdult.ToString(), year ?? string.Empty));
            return response.FromJson<TMDbMovieSearch>();
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
