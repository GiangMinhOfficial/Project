using System;
using System.Collections.Generic;

namespace ProductNews.Models
{
    public partial class News
    {
        public News()
        {
            Comments = new HashSet<Comment>();
            Evaluations = new HashSet<Evaluation>();
        }

        public int NewsId { get; set; }
        public int? NewsGroupId { get; set; }
        public string? NewsTitle { get; set; }
        public string? NewsHeading { get; set; }
        public string? NewsPreviewImage { get; set; }
        public string? NewsContent { get; set; }
        public int? View { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedHistory { get; set; }
        public bool? IsDelete { get; set; }

        public virtual Account? CreatedByNavigation { get; set; }
        public virtual Account? ModifiedByNavigation { get; set; }
        public virtual NewsGroup? NewsGroup { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
