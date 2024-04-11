using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.User.Infrastructure;
using Artificial.Scrum.Master.User.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.User.Features.Authorization
{
    internal interface IAuthorizationService
    {
        UserInfo Handle();
    }

    internal class AuthorizationService(IUserAccessor _userAccessor) : IAuthorizationService
    {
        public UserInfo Handle()
        {
            return new UserInfo()
            {
                UserId = _userAccessor.UserId ?? "",
                UserName = _userAccessor.UserName ?? "",
                PhotoUrl = _userAccessor.PhotoUrl ?? ""
            };
        }
    }
}
