using System;
using System.Collections.Generic;

namespace ProductNews.Models
{
    public partial class NewsGroup
    {
        public NewsGroup()
        {
            News = new HashSet<News>();
        }

        public int NewsGroupId { get; set; }
        public string? NewsGroupName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedHistory { get; set; }
        public bool? IsDelete { get; set; }

        public virtual Account? CreatedByNavigation { get; set; }
        public virtual Account? ModifiedByNavigation { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
