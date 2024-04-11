using Artificial.Scrum.Master.Interfaces;

namespace Artificial.Scrum.Master.Infrastructure
{
    public class MockedUserAccessor : IUserAccessor
    {
        public string UserId => "1";
        public string UserName => "Test User";
        public string PhotoUrl => "https://c.tenor.com/UhjesZys6mAAAAAC/tenor.gif";
    }
}
