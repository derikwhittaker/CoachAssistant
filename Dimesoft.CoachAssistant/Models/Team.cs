using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Team : ViewModelBase
    {
        private readonly TeamDto _dto;
        private bool _selected;

        public Team( TeamDto dto)
        {
            _dto = dto;
        }

        public int Id
        {
            get { return _dto.Id; }
            set { _dto.Id = value; RaisePropertyChanged(() => Id); }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged(() => Selected); }
        }

        public string Name
        {
            get { return _dto.Name; }
            set { _dto.Name = value; RaisePropertyChanged(() => Name); }
        }
    }
}