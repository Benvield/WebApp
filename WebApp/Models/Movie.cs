using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Movie
    {
        public MovieInfo movieInfo { get; set; }
        public List<string> genreList { get; set; }
        public string plot { get; set; }
        public string errorMessage { get; set; }

        public Movie()
        {
            movieInfo = new MovieInfo();
            genreList = new List<string>();
        }
    }
}