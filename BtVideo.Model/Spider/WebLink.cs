using System;
using System.ComponentModel.DataAnnotations;

namespace BtVideo.Model.Spider
{
    public class WebLink
    {
        [Key]
        public string Guid { get; set; }

        public string Url { get; set; }

        public DateTime Dt { get; set; }

        public bool IsGeted { get; set; }
    }
}
