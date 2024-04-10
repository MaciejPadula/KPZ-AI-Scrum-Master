using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.User.Infrastructure
{
    internal class JwtTokenResponse
    {
        public string? Token { get; set; }
        public bool Success { get; set; }
    }
}
