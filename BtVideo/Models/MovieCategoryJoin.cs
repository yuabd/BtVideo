using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BtVideo.Models
{
    public class MovieCategoryJoin
    {
        [Key]
        [Column(Order = 1)]
        public int MovieID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CategoryID { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual MovieCategory MovieCategory { get; set; }
    }
}