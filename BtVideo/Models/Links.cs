using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BtVideo.Models
{
    public class Links
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "必填"), MaxLength(100)]
        public string LinkUrl { get; set; }
        [Required(ErrorMessage = "必填")]
        public int? SortOrder { get; set; }
        [Required(ErrorMessage = "必填"), MaxLength(100)]
        public string Name { get; set; }

        public string PictureFile { get; set; }

        public string Description { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public DateTime? DateCreated { get; set; }

        //public string UpdateUser { get; set; }
    }
}