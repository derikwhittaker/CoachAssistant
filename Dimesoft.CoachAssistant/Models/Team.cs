using System.Collections.Generic;
using System.Linq;
using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Team : ViewModelBase
    {
        public readonly TeamDto Dto;
        private readonly SportDto _sport;
        private bool _selected;
        private IList<Player> _players;

        public Team(TeamDto dto, SportDto sport)
        {
            Dto = dto;
            _sport = sport;

            Players = new List<Player>();
            if ( dto.Players != null )
            {
                Players = dto.Players.Select(x => new Player(x)).ToList();
            }
        }

        public int Id
        {
            get { return Dto.Id; }
            set { Dto.Id = value; RaisePropertyChanged(() => Id); }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged(() => Selected); }
        }

        public string Name
        {
            get { return Dto.Name; }
            set { Dto.Name = value; RaisePropertyChanged(() => Name); }
        }

        public bool MyTeam
        {
            get { return Dto.MyTeam; }
            set { Dto.MyTeam = value; RaisePropertyChanged(() => MyTeam); }
        }

        public string SportName
        {
            get { return _sport.Name; }
        }

        public string MyTeamDescription
        {
            get 
            {
                return Dto.MyTeam ? "[My Team]" : string.Empty;
            }
        }

        public IList<Player> Players
        {
            get { return _players; }
            set
            {
                _players = value;

                RaisePropertyChanged(() => Players);
            }
        }
    }

    public class Player
    {
        public readonly PlayerDto Dto;

        public Player(PlayerDto dto)
        {
            Dto = dto;
        }

        public string PlayerName
        {
            get { return string.Format("{0} {1}", Dto.FirstName, Dto.LastName); }
        }

        public int JerseyNumber
        {
            get { return Dto.JerseyNumber; }
        }
    }
}