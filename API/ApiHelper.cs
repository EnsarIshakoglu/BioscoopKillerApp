using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace API
{
    public class ApiHelper
    {
        private const string ApiKey = "883e4889";

        public async Task AddApiDataToMovie(Movie movie)
        {
            string movieTitleForApi = movie.Title.Replace(" ", "%3A");
            string url = $"http://www.omdbapi.com/?apikey={ ApiKey}&t={ movieTitleForApi}&y={ movie.PublishedYear}";

            HttpClient client = new HttpClient();
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Movie apiDataMovie = await response.Content.ReadAsAsync<Movie>();
                        movie.Poster = apiDataMovie.Poster;
                        movie.Plot = apiDataMovie.Plot;
                        movie.Title = apiDataMovie.Title;
                        movie.Runtime = apiDataMovie.Runtime;
                        movie.Actors = apiDataMovie.Actors;
                        movie.Genre = apiDataMovie.Genre;
                    }
                }
            }
            catch (Exception ex)
            {
                //File.AppendAllText(, message);
            }
        }
    }
}
