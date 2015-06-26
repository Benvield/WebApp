using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WebApp.Models
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "Plot")]
        public string Plot { get; set; }
    }
}