using System;
using System.Collections.Generic;

namespace Dimesoft.CoachAssistant.Domain.Models
{
    public class EventDto : IBaseModel
    {
        public EventDto()
        {
            Team = new TeamDto();
            Location = new LocationDto();
            PracticeDrills = new List<PracticeDrillDto>();
        }

        public int Id { get; set; }
        
        public TeamDto Team { get; set; }

        public TeamDto Opponent { get; set; }

        public LocationDto Location { get; set; }

        public DateTime Date { get; set; }

        public int EventTypeId { get; set; }

        [Wintellect.Sterling.Serialization.SterlingIgnore]
        public EventType EventType 
        {
            get { return (EventType)Enum.Parse(typeof(EventType), EventTypeId.ToString(), true); } 
        }

        public int SportTypeId { get; set; }

        [Wintellect.Sterling.Serialization.SterlingIgnore]
        public SportType SportType
        {
            get { return (SportType)Enum.Parse(typeof(SportType), SportTypeId.ToString(), true); }
        }

        public string Notes { get; set; }
        
        public bool Completed { get; set; }

        public IList<PracticeDrillDto> PracticeDrills { get; set; }

    }

    public class TeamDto : IBaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SportTypeId { get; set; }

    }

    public class LocationDto : IBaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
    
    public class PracticeDrillDto : IBaseModel
    {        
        public int Sequence { get; set; }

        public int Id { get; set; }

        public int SportTypeId { get; set; }

        [Wintellect.Sterling.Serialization.SterlingIgnore]
        public SportType SportType
        {
            get { return (SportType)Enum.Parse(typeof(SportType), SportTypeId.ToString(), true); }
        }

        public string Name { get; set; }
        
        public string Notes { get; set; }
    }


    public enum SportType
    {
        Unknown = 0,
        Soccer = 1,
        Baseball = 2,
        Basketball = 3
    }

    public enum EventType
    {
        Unknown = 0,
        Practice = 1,
        Game = 2
    }
}
