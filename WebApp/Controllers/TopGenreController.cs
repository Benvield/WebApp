using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using Npgsql;
using System.Configuration;
using System.Web.Helpers;

namespace WebApp.Controllers
{
    public class TopGenreController : Controller
    {
        //
        // GET: /TopGenre/
        private int test;

        public ActionResult Index()
        {
            TopGenre genreModel = new TopGenre();
            Genre newGenre;
            test = 3;

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            //NpgsqlConnection connection = new NpgsqlConnection("");
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                genreModel.errorMessage = "Couldn't connect to server";
            }

            if (String.IsNullOrEmpty(genreModel.errorMessage))
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand("SELECT g.name,Count(mg.movie_id) FROM movie_genre AS mg, genre AS g WHERE mg.genre_id = g.id GROUP BY g.name ORDER BY g.name;", connection);
                    NpgsqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        newGenre = new Genre
                        {
                            name = (string)dataReader[0],
                            popularity = (long)dataReader[1],
                        };

                        genreModel.genreList.Add(newGenre);
                    }                    
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(genreModel);
        }
                
        //public ActionResult MakeChart(List<Genre> genreList)
        //{
        //    return View(genreList);
        //}

    }
}
