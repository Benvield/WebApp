using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using Npgsql;
using System.Net;
using System.Diagnostics;
using System.Configuration;
using System.Runtime.Serialization.Json;

namespace WebApp.Controllers
{
    public class MovieController : Controller
    {       
        //
        // GET: /Movie/
        public ActionResult Index(int? movieId)
        {
            Movie movieModel = new Movie();
            string genre;

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                movieModel.errorMessage = "Couldn't connect to server";
            }

            if (String.IsNullOrEmpty(movieModel.errorMessage))
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand("SELECT id,vote_average,title FROM movie WHERE id = " + movieId + ";", connection);
                    NpgsqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        movieModel.movieInfo.id = (int)dataReader[0];
                        movieModel.movieInfo.voteAverage = (float)dataReader[1];
                        movieModel.movieInfo.title = (string)dataReader[2];
                    }

                    command = new NpgsqlCommand("SELECT g.name FROM genre AS g, movie_genre AS mg WHERE g.id = mg.genre_id AND mg.movie_id = " + movieId + ";", connection);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        genre = String.Copy((string)dataReader[0]);

                        movieModel.genreList.Add(genre);
                    }
                }
                finally
                {
                    connection.Close();
                }

                movieModel.plot = getPlot(movieModel.movieInfo.title);
            }

            return View(movieModel);
        }

        private string getPlot(string title)
        {
            try
            {
                string plotRequest = CreateRequest(title);
                Response plotResponse = MakeRequest(plotRequest);
                if (plotResponse != null)
                {
                    return plotResponse.Plot;
                } else
                {
                    return "Couldn't find film";
                }
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return "";
        }

        public static string CreateRequest(string title)
        {
            string UrlRequest = "http://www.omdbapi.com/" +
                                 "?t=" + title + "&plot=short&r=json";
            return (UrlRequest);
        }

        public static Response MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    }
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    Response jsonResponse = objResponse as Response;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

    }
}
