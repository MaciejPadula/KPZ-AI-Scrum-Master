namespace Artificial.Scrum.Master.SharedKernel;

public interface ISessionKeyGenerator
{
    string Key { get; }
}

internal class SessionKeyGenerator : ISessionKeyGenerator
{
    public string Key => Guid.NewGuid().ToString().Replace("-", "");
}
