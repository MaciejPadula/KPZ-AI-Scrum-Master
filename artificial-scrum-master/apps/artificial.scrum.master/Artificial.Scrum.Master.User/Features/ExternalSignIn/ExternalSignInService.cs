using Artificial.Scrum.Master.User.Infrastructure;
using Artificial.Scrum.Master.User.Infrastructure.JWT;
using Artificial.Scrum.Master.User.Infrastructure.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace Artificial.Scrum.Master.User.Features.ExternalSignIn
{
    internal interface IExternalSignInService
    {
        Task<JwtTokenResponse> Handle(string token);
    }
    internal class ExternalSignInService(IJwtAuthorizationService _jwtAuthorizationService) : IExternalSignInService
    {
        public async Task<JwtTokenResponse> Handle(string idToken)
        {
            try
            {
                if (string.IsNullOrEmpty(idToken))
                {
                    return new JwtTokenResponse();
                }

                var validatedToken = await GoogleJsonWebSignature.ValidateAsync(idToken);

                var jwtToken = _jwtAuthorizationService.GenerateJwtToken(
                    new UserInfo()
                    {
                        UserId = validatedToken.Subject,
                        PhotoUrl = validatedToken.Picture,
                        UserName = validatedToken.Name
                    });
                return new JwtTokenResponse
                {
                    Token = jwtToken
                };
            }
            catch
            {
                return new JwtTokenResponse();
            }
        }
    }
}
