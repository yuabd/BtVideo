using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BtVideo.Models
{
    public class HotKeyword
    {
        [Key]
        public string Keyword { get; set; }

        public int Count { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}