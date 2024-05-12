using Artificial.Scrum.Master.Retrospectives.Features.Shared.Models;
using System.Runtime.Serialization;

namespace Artificial.Scrum.Master.Retrospectives.Features.CreateSessionCard;
[Serializable]
internal class InvalidCardTypeException : Exception
{
    public InvalidCardTypeException(CardType type) : base($"Invalid card type: {type}")
    {
    }
}
