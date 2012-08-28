using System;

namespace Dimesoft.CoachAssistant.Domain.Models
{
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

        public int ExpectedDurationInMinutes { get; set; }
    }
}