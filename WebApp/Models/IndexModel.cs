using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class IndexModel
    {
        //public int ID { get; set; }
        //public string Description { get; set; }
        //public string Comments { get; set; }

        public List<MovieInfo> movieList { get; set; }
        public string errorMessage { get; set; }
        public IndexModel()
        {
            movieList = new List<MovieInfo>();
        }
        
    }

    public class MovieInfo
    {
        public int id { get; set; }
        //public string originalLanguage { get; set; }
        //public string originalTitle { get; set; }
        //public DateTime releaseDate { get; set; }
        public float voteAverage { get; set; }
        public string title { get; set; }
    }
}