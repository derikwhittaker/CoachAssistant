
using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Event : ViewModelBase
    {
        protected readonly EventDto _dto;


        public Event(EventDto dto)
        {
            _dto = dto;
        }

        public EventType EventType
        {
            get { return _dto.EventType; }
        }

        public SportType SportType
        {
            get { return _dto.SportType; }
        }

        public string EventName
        {
            get
            {
                if (EventType == EventType.Game)
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
            get { return _dto.Team.Name; }
        }

        public string OpponentTeamName
        {
            get
            {
                if (_dto.Opponent != null)
                {
                    return _dto.Opponent.Name;    
                }

                return string.Empty;
            }
        }

        public string LocationName
        {
            get { return _dto.Location.Name; }
        }
        
        public string Date
        {
            get { return _dto.Date.ToString(); }
        }


        public object Id
        {
            get { return _dto.Id; }
        }
    }
}
