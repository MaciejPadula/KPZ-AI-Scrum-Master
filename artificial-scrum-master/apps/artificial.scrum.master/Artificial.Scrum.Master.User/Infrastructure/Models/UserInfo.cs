using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.User.Infrastructure.Models
{
    internal class UserInfo
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string PhotoUrl { get; set; }
    }
}
