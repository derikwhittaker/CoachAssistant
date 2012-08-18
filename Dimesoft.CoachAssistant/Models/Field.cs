using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Field : ViewModelBase
    {
        private readonly LocationDto _dto;
        private bool _selected;

        public Field(LocationDto dto)
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