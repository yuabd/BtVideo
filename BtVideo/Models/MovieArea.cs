using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BtVideo.Models
{
    public class MovieArea
    {
        [Key]
        public int AreaID { get; set; }

        public string AreaName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}