using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtVideo.Models
{
    public class MovieLink
    {
        [Key]
        public int LinkID { get; set; }

        public int MovieID { get; set; }

        [MaxLength(100), Required]
        public string LinkName { get; set; }

        /// <summary>
        /// 下载链接
        /// </summary>
        [Required,MaxLength(300)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 磁力链接
        /// </summary>
        [MaxLength(500)]
        public string Magnet { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadCount { get; set; }

        public virtual Movie Movie { get; set; }

        [NotMapped]
        public string PictureFolder { get { return "/Content/Bt/" + MovieID; } }
    }
}