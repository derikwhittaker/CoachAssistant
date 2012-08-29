
using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Event : ViewModelBase
    {
        public readonly EventDto Dto;

        public Event(EventDto dto)
        {
            Dto = dto;
        }

        public Domain.Models.EventType EventType
        {
            get { return Dto.EventType; }
            set { Dto.EventTypeId = (int)value; }
        }

        public SportType SportType
        {
            get { return Dto.SportType; }
        }

        public string EventName
        {
            get
            {
                if (EventType == Domain.Models.EventType.Game)
                {
                    return string.Format("{0} vs. {1}", TeamName, OpponentTeamName);
                }
                else
                {
                    return TeamName;
                }
            }
        }

        public string TeamName
        {
            get { return Dto.Team.Name; }
        }

        public string OpponentTeamName
        {
            get
            {
                if (Dto.Opponent != null)
                {
                    return Dto.Opponent.Name;    
                }

                return string.Empty;
            }
        }

        public string LocationName
        {
            get { return Dto.Location.Name; }
        }
        
        public string Date
        {
            get { return Dto.Date.ToString("g"); }
        }

        public string Notes
        {
            get { return Dto.Notes; }
        }

        public int Id
        {
            get { return Dto.Id; }
        }

        public bool IsCompleted
        {
            get { return Dto.Completed; }
            set { Dto.Completed = value; }
        }
    }
}
