using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtVideo.Models
{
	public class Movie
	{
        [Key]
		public int MovieID { get; set; }
		[MaxLength(70)]
		[Required]
		public string MovieTitle { get; set; }
		[Column(TypeName = "ntext")]
		[MaxLength]
		public string MovieContent { get; set; }
		[MaxLength(15)]
		[Required]
		public string AuthorID { get; set; }
		[MaxLength(200)]
		public string PictureFile { get; set; }

		public bool IsPublic { get; set; }

		public int PageVisits { get; set; }

        public int SortOrder { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public int AreaID { get; set; }
        /// <summary>
        /// 演员
        /// </summary>
        public string Stars { get; set; }
        /// <summary>
        /// 导演
        /// </summary>
        public string Director { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime DateUpdate { get; set; }
        /// <summary>
        /// 上映时间
        /// </summary>
        public DateTime ShowDate { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public double Grade { get; set; }
        /// <summary>
        /// 磁力链接
        /// </summary>
        [MaxLength(500)]
        public string Magnet { get; set; }
        /// <summary>
        /// Imdb链接
        /// </summary>
        [MaxLength(200)]
        public string ImdbLink { get; set; }
        /// <summary>
        /// Imdb title
        /// </summary>
        [MaxLength(50)]
        public string ImdbTitle { get; set; }

        //SEO
        [MaxLength(100)]
		public string PageTitle { get; set; }
		[MaxLength(500)]
		public string MetaDescription { get; set; }
		[MaxLength(300)]
		public string MetaKeywords { get; set; }
		[MaxLength(56)]	//50 + "-" + 99,000
		public string Slug { get; set; }
        
        public virtual ICollection<MovieComment> MovieComments { get; set; }
       
        public virtual ICollection<MovieLink> MovieLinks { get; set; }
       
        public virtual ICollection<MovieTag> MovieTags { get; set; }

        public virtual ICollection<MovieCategoryJoin> MovieCategoryJoins { get; set; }

        public virtual ICollection<MovieStarJoin> MovieStarJoins { get; set; }

        public virtual MovieArea MovieArea { get; set; }

        [NotMapped]
        public string PictureThumbnail
        {
            get
            {
                if (PictureFile.StartsWith("http"))
                {
                    return PictureFile;
                }
                else
                {
                    return PictureFolder + "/" + (string.IsNullOrEmpty(PictureFile) ? "default.jpg" : PictureFile);
                }
            }
        }
        [NotMapped]
        public string PictureFolder { get { return "/Content/Pictures/Blog"; } }

    }

    public class PreNextBlog
	{
		public BlogIDName PreBlog { get; set; }

		public BlogIDName NextBlog { get; set; }
	}

	public class BlogIDName
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public string Slug { get; set; }
	}
}