using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Team : ViewModelBase
    {
        public readonly TeamDto Dto;
        private readonly Sport _sport;
        private bool _selected;

        public Team(TeamDto dto, Sport sport)
        {
            Dto = dto;
            _sport = sport;
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
    }
}