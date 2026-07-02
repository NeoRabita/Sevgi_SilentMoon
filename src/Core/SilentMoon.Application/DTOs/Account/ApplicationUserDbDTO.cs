using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.DTOs.Account
{
    public class ApplicationUserDbDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int IsEmailConfirmed { get; set; }= 0;
        public DateTime CreatedAt { get; set; }
    }
}
