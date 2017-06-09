using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BtVideo.Models
{
    public class MovieStarJoin
    {
        [Key]
        [Column(Order=1)]
        public int MovieID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int StarID { get; set; }
        /// <summary>
        /// 类型：导演、演员、
        /// </summary>
        [MaxLength(50), Required]
        public string Type { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual MovieStar MovieStar { get; set; }
    }
}