using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtVideo.Models
{
	public class MovieCategory
	{
		[Key]
		public int CategoryID { get; set; }
		[MaxLength(25)]
		[Required]
		public string CategoryName { get; set; }
		//SEO
		[MaxLength(70)]
		public string PageTitle { get; set; }
		[MaxLength(300)]
		public string MetaDescription { get; set; }
		[MaxLength(100)]
		public string MetaKeywords { get; set; }
		[MaxLength(56)]	//50 + "-" + 99,000
		public string Slug { get; set; }

		public int SortOrder { get; set; }
        
        public virtual ICollection<MovieCategoryJoin> MovieCategoryJoins { get; set; }
    }
}