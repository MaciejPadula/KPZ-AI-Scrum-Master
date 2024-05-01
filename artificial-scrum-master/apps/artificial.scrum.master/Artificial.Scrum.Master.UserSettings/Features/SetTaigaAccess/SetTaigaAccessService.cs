using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.UserSettings.Features.Shared;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;

namespace Artificial.Scrum.Master.UserSettings.Features.SetTaigaAccess;

internal interface ISetTaigaAccessService
{
    Task Handle(SetTaigaAccessRequest request);
}

internal class SetTaigaAccessService : ISetTaigaAccessService
{
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly IUserAccessor _userAccessor;

    public SetTaigaAccessService(
        IUserSettingsRepository userSettingsRepository,
        IUserAccessor userAccessor)
    {
        _userSettingsRepository = userSettingsRepository;
        _userAccessor = userAccessor;
    }

    public async Task Handle(SetTaigaAccessRequest request)
    {
        var userId = _userAccessor.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException();
        }

        var entity = await _userSettingsRepository.GetUserSettings(userId);

        if (entity is not null)
        {
            await _userSettingsRepository.UpdateUserSettings(entity with
            {
                TaigaAccessToken = request.AccessToken,
                TaigaRefreshToken = request.RefreshToken
            });
        }
        else
        {
            await _userSettingsRepository.AddUserSettings(new UserSettingsEntity(
                _userAccessor.UserId,
                request.AccessToken,
                request.RefreshToken));
        }
    }
}
