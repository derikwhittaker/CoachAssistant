using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class Drill : ViewModelBase
    {
        private readonly PracticeDrillDto _dto;
        private int _id;
        private bool _selected;
        private string _name;

        public Drill(PracticeDrillDto dto)
        {
            _dto = dto;
        }

        public int Id
        {
            get { return _dto.Id; }
            set { _dto.Id = value; RaisePropertyChanged(() => Id); }
        }

        public int SportTypeId
        {
            get { return _dto.SportTypeId; }
            set { _dto.SportTypeId = value; RaisePropertyChanged(() => SportTypeId); }
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

        public override string ToString()
        {
            if ( _dto == null )
            {
                return "";
            }

            return string.Format("ID: {0} Name: {1} Selected: {2}", Id, Name, Selected);
        }
    }
}