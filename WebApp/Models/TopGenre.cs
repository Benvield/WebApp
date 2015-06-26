﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TopGenre
    {
        public List<Genre> genreList;
        public string errorMessage { get; set; }

        public TopGenre()
        {
            genreList = new List<Genre>();
        }
    }

    public class Genre
    {
        public string name { get; set; }
        public long popularity { get; set; }
    }
}