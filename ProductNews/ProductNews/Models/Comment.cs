using System;
using System.Collections.Generic;

namespace ProductNews.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? NewsId { get; set; }
        public int? CustomerId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public bool? IsDelete { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual News? News { get; set; }
    }
}
