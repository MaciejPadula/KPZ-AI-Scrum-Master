using Artificial.Scrum.Master.Interfaces;

namespace Artificial.Scrum.Master.Infrastructure
{
    public class MockedUserAccessor : IUserAccessor
    {
        public string UserId => "1";
    }
}
