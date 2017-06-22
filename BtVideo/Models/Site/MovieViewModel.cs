using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtVideo.Models.Site
{
    public class MovieViewModel
    {
        public int MovieID { get; set; }

        public string MovieTitle { get; set; }

        public string MovieContent { get; set; }

        public string Stars { get; set; }

        public string Director { get; set; }

        public string PictureFile { get; set; }

        public int PageVisits { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public double Grade { get; set; }

        public IndexType IT
        {
            get;
            set;
        }
    }

    //操作类型枚举
    public enum IndexType
    {
        Insert,
        Modify,
        Delete
    }
}