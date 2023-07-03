using System;
using System.Collections.Generic;

namespace ProductNews.Models
{
    public partial class Account
    {
        public Account()
        {
            NewsCreatedByNavigations = new HashSet<News>();
            NewsGroupCreatedByNavigations = new HashSet<NewsGroup>();
            NewsGroupModifiedByNavigations = new HashSet<NewsGroup>();
            NewsModifiedByNavigations = new HashSet<News>();
        }

        public int AccountId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<News> NewsCreatedByNavigations { get; set; }
        public virtual ICollection<NewsGroup> NewsGroupCreatedByNavigations { get; set; }
        public virtual ICollection<NewsGroup> NewsGroupModifiedByNavigations { get; set; }
        public virtual ICollection<News> NewsModifiedByNavigations { get; set; }
    }
}
