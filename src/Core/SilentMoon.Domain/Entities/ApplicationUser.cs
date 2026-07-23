using SilentMoon.Domain.Common;
using System;
using System.Collections.Generic;

namespace SilentMoon.Domain.Entities
{
    public class ApplicationUser:BaseEntity<string>
    {

         public string FirstName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        //public string LastName { get; set; }
        public int? RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public ICollection<UserTopic> UserTopics { get; set; } = new List<UserTopic>();
    }

}
