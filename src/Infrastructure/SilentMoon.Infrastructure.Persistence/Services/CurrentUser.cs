using SilentMoon.Application.Common.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class CurrentUser : ICurrentUser
    {
        public string Email { get; set; }
    }
}
