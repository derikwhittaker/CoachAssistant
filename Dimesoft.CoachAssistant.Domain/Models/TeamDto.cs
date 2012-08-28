namespace Dimesoft.CoachAssistant.Domain.Models
{
    public class TeamDto : IBaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SportTypeId { get; set; }

    }
}