using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Field : ViewModelBase
    {
        public readonly LocationDto Dto;
        private bool _selected;

        public Field(LocationDto dto)
        {
            Dto = dto;
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
    }
}