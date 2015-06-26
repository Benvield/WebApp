using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using Npgsql;
using System.Diagnostics;
using System.Configuration;

namespace WebApp.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/
        public ActionResult Index()
        {
            IndexModel indexModel = new IndexModel();    
            MovieInfo newMovie;
            
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                indexModel.errorMessage = "Couldn't connect to server";
            }

            if (String.IsNullOrEmpty(indexModel.errorMessage))
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand("SELECT id,vote_average,title FROM movie WHERE vote_count > 100 ORDER BY vote_average DESC, release_date LIMIT 20;", connection);
                    NpgsqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        newMovie = new MovieInfo
                        {
                            id = (int)dataReader[0],
                            voteAverage = (float)dataReader[1],
                            title = (string)dataReader[2]
                        };

                        indexModel.movieList.Add(newMovie);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(indexModel);
        }


        public string Test()
        {
            return HttpUtility.HtmlEncode("Hello World");
        }
    }
}
