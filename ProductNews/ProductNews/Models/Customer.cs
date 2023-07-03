using System;
using System.Collections.Generic;

namespace ProductNews.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Comments = new HashSet<Comment>();
            Evaluations = new HashSet<Evaluation>();
        }

        public int CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Avatar { get; set; }
        public string? ModifiedHistory { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
