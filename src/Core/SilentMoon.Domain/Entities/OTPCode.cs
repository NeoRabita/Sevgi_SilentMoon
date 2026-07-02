using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Domain.Entities
{
    public class OTPCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }     
        public DateTime ExpiresAt { get; set; }     
        public int Attempts { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
        public bool CanAttempt => Attempts < 5;

    }
}
