using Artificial.Scrum.Master.Interfaces;
using Artificial.Scrum.Master.UserSettings.Infrastructure;
using Artificial.Scrum.Master.UserSettings.Infrastructure.Models;

namespace Artificial.Scrum.Master.UserSettings.Features.GetUserSettings;

internal interface IGetUserSettingsService
{
    Task<GetUserSettingsResponse> Handle();
}

internal class GetUserSettingsService : IGetUserSettingsService
{
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly IUserAccessor _userAccessor;

    public GetUserSettingsService(
        IUserSettingsRepository userSettingsRepository,
        IUserAccessor userAccessor)
    {
        _userSettingsRepository = userSettingsRepository;
        _userAccessor = userAccessor;
    }

    public async Task<GetUserSettingsResponse> Handle()
    {
        var result = await _userSettingsRepository.GetUserSettings(_userAccessor.UserId);
        if (result is null)
        {
            return new();
        }

        return new GetUserSettingsResponse(ValidateTaigaAccess(result));
    }

    private bool ValidateTaigaAccess(UserSettingsEntity? userSettings)
    {
        return userSettings is not null
            && !string.IsNullOrEmpty(userSettings.TaigaAccessToken)
            && !string.IsNullOrEmpty(userSettings.TaigaRefreshToken);
    }
}
