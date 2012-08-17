
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

        public string TeamName
        {
            get { return _dto.Team.Name; }
        }

        public string LocationName
        {
            get { return _dto.Location.Name; }
        }
        
        public string Date
        {
            get { return _dto.Date.ToString(); }
        }

        public string Time
        {
            get { return _dto.Time.ToString(); }
        }

        public object Id
        {
            get { return _dto.Id; }
        }
    }
}
