using Dimesoft.CoachAssistant.Domain.Models;

namespace Dimesoft.CoachAssistant.Models
{
    public class GameEvent : Event
    {
        public GameEvent(EventDto dto) : base(dto)
        {
        }
    }
}