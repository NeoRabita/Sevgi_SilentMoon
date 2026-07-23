using System;
using System.ComponentModel.DataAnnotations.Schema;
using SilentMoon.Domain.Common;

namespace SilentMoon.Domain.Entities
{
    [Table("RefreshTokens")]
    public class RefreshToken : BaseEntity<int>
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsExpired && !IsRevoked;
    }
}