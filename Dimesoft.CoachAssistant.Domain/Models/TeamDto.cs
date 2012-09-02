using System;
using System.Collections.Generic;

namespace Dimesoft.CoachAssistant.Domain.Models
{
    public class TeamDto : IBaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SportTypeId { get; set; }

        public bool MyTeam { get; set; }

        public IList<PlayerDto> Players { get; set; }
    }

    public class PlayerDto : IBaseModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Nickname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int JerseyNumber { get; set; }
    }

    public class SportDto : IBaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}