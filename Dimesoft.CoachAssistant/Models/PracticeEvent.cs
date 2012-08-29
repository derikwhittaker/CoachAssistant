using System.Linq;
using System.Collections.Generic;
using Dimesoft.CoachAssistant.Domain.Models;
using GalaSoft.MvvmLight;

namespace Dimesoft.CoachAssistant.Models
{
    public class PracticeEvent : Event
    {
        public PracticeEvent(EventDto dto) : base(dto)
        {
            if (dto.PracticeDrills != null)
            {
                PracticeDrills = dto.PracticeDrills.Select(x => new PracticeDrill(x)).ToList();    
            }
        }

        private IList<PracticeDrill> _practiceDrills = new List<PracticeDrill>();
        public IList<PracticeDrill> PracticeDrills
        {
            get { return _practiceDrills; }
            set { _practiceDrills = value; }
        }

    }

    public class PracticeDrill : ViewModelBase
    {
        private readonly PracticeDrillDto _dto;
        private bool _selected;

        public PracticeDrill(PracticeDrillDto dto)
        {
            if (dto == null)
            {
                var x = 1;
            }
            _dto = dto ?? new PracticeDrillDto();
        }


        public int Id
        {
            get { return _dto.Id; }
        }

        public string Name
        {
            get { return _dto.Name; }
            set { _dto.Name = value; RaisePropertyChanged(() => Name); }
        }

        public int Sequence
        {
            get { return _dto.Sequence; }
            set { _dto.Sequence = value; RaisePropertyChanged(() => Sequence); }
        }

        public int ExpectedDuration
        {
            get { return _dto.ExpectedDurationInMinutes; }
            set { _dto.ExpectedDurationInMinutes = value; RaisePropertyChanged(() => ExpectedDuration); }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged(() => Selected); }
        }
    }

    public enum SelectionState
    {
        Unknown = 0,
        Unselected = 1,
        Selected = 2
    }

}