using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using Npgsql;
using System.Configuration;
using System.Globalization;

namespace WebApp.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/        
        public ActionResult Index()
        {
            Search searchModel = new Search();
            return View(searchModel);
        }


        //[HttpPost]
        //public ActionResult Index(string popularitySearch, string genreSearch)
        //{
        //    Search searchModel = new Search();
        //    MovieInfo newMovie;

        //    NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["connectionString"]);
        //    try
        //    {
        //        connection.Open();
        //    }
        //    catch (Exception e)
        //    {
        //        searchModel.errorMessage = "Couldn't connect to server";
        //    }

        //    if (String.IsNullOrEmpty(searchModel.errorMessage))
        //    {

        //        if (!String.IsNullOrEmpty(popularitySearch) || !String.IsNullOrEmpty(genreSearch))
        //        {
        //            NpgsqlCommand command;
        //            if (!String.IsNullOrEmpty(popularitySearch) && !String.IsNullOrEmpty(genreSearch))
        //            {
        //                command = new NpgsqlCommand("SELECT m.id,m.popularity,m.title FROM movie_genre AS mg, genre AS g, movie AS m WHERE mg.genre_id = g.id AND mg.movie_id = m.id AND m.popularity > " + popularitySearch + " AND g.name = '" + genreSearch + "' ORDER BY m.popularity DESC, m.release_date;", connection);
        //            }
        //            else if (!String.IsNullOrEmpty(popularitySearch) && String.IsNullOrEmpty(genreSearch))
        //            {
        //                command = new NpgsqlCommand("SELECT id,popularity,title FROM movie WHERE popularity > " + popularitySearch + " ORDER BY popularity DESC, release_date;", connection);
        //            }
        //            else
        //            {
        //                command = new NpgsqlCommand("SELECT m.id,m.popularity,m.title FROM movie_genre AS mg, genre AS g, movie AS m WHERE mg.genre_id = g.id AND mg.movie_id = m.id AND g.name = '" + genreSearch + "' ORDER BY m.popularity DESC, m.release_date;", connection);
        //            }

        //            try
        //            {
        //                NpgsqlDataReader dataReader = command.ExecuteReader();
        //                while (dataReader.Read())
        //                {
        //                    newMovie = new MovieInfo
        //                    {
        //                        id = (int)dataReader[0],
        //                        popularity = (float)dataReader[1],
        //                        title = (string)dataReader[2]
        //                    };

        //                    searchModel.searchResult.Add(newMovie);
        //                }
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }

        //    return View(searchModel);
        //}

        [HttpPost]
        public ActionResult Index(Search searchModel)
        {
            MovieInfo newMovie;
            
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                searchModel.errorMessage = "Couldn't connect to server";
            }

            if (String.IsNullOrEmpty(searchModel.errorMessage))
            {

                if (searchModel.voteAverageSearch!=0 || !String.IsNullOrEmpty(searchModel.genreSearch))
                {
                    NpgsqlCommand command;
                    if (searchModel.voteAverageSearch!=0  && !String.IsNullOrEmpty(searchModel.genreSearch))
                    {
                        command = new NpgsqlCommand("SELECT m.id,m.vote_average,m.title FROM movie_genre AS mg, genre AS g, movie AS m WHERE mg.genre_id = g.id AND mg.movie_id = m.id AND m.vote_average > " + Convert.ToString(searchModel.voteAverageSearch, CultureInfo.InvariantCulture.NumberFormat) + " AND g.name = '" + searchModel.genreSearch + "' ORDER BY m.vote_average DESC, m.release_date;", connection);
                    }
                    else if (searchModel.voteAverageSearch!=0  && String.IsNullOrEmpty(searchModel.genreSearch))
                    {
                        command = new NpgsqlCommand("SELECT id,vote_average,title FROM movie WHERE vote_average > " + Convert.ToString(searchModel.voteAverageSearch, CultureInfo.InvariantCulture.NumberFormat) + " ORDER BY vote_average DESC, release_date;", connection);
                    }
                    else
                    {
                        command = new NpgsqlCommand("SELECT m.id,m.vote_average,m.title FROM movie_genre AS mg, genre AS g, movie AS m WHERE mg.genre_id = g.id AND mg.movie_id = m.id AND g.name = '" + searchModel.genreSearch + "' ORDER BY m.vote_average DESC, m.release_date;", connection);
                    }

                    try
                    {
                        NpgsqlDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            newMovie = new MovieInfo
                            {
                                id = (int)dataReader[0],
                                voteAverage = (float)dataReader[1],
                                title = (string)dataReader[2]
                            };

                            searchModel.searchResult.Add(newMovie);
                        }

                        if (searchModel.searchResult.Count == 0)
                        {
                            searchModel.errorMessage = "Couldn't find any matching films";
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return View(searchModel);
        }

    }
}
