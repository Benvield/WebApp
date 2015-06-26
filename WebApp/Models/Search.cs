using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Search
    {
        public List<MovieInfo> searchResult { get; set; }
        public string errorMessage { get; set; }        
        public string genreSearch { get; set; }
                
        [DisplayFormat(ApplyFormatInEditMode = true , DataFormatString = "{0:0.0}")]
        public float voteAverageSearch { get; set; }

        public Search()
        {
            searchResult = new List<MovieInfo>();
        }        
    }
}