using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Common.User
{
    public interface ICurrentUser
    {
        string Email { get; set; }

    }
}
