using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BtVideo.Models
{
    public class MovieStar
    {
        [Key]
        public int StarID { get; set; }

        public string StarName { get; set; }

        public virtual ICollection<MovieStarJoin> MovieStarJoins { get; set; }
    }
}