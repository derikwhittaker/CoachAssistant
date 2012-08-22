using System;
using System.Collections.Generic;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;

namespace Dimesoft.CoachAssistant.ViewModels.Practice
{
    public class DrillCreationViewModel : BaseVM
    {
        private int _currentSportId = 0;
        private string _callingPageName;
        private IList<Sport> _sports;
        private Sport _selectedSport;
        private readonly IDrillsRepository _drillsRepository;
        private readonly ISessonStateService _sessonStateService;
        private readonly INavigationService _navigationService;
        private string _sportName;
        private string _drillName;
        private string _drillNotes;
        private RelayCommand _saveNewDrillCommand;

        public DrillCreationViewModel(IDrillsRepository drillsRepository, ISessonStateService sessonStateService, INavigationService navigationService)
        {
            _drillsRepository = drillsRepository;
            _sessonStateService = sessonStateService;
            _navigationService = navigationService;

            PageTitle = "New Practice Drill";

            Sports = new List<Sport>
                         {
                             new Sport {Id = (int) SportType.Unknown, Name = "No Selection Made"},
                             new Sport {Id = (int) SportType.Soccer, Name = "Soccer"},
                             new Sport {Id = (int) SportType.Baseball, Name = "Baseball"},
                             new Sport {Id = (int) SportType.Basketball, Name = "Basketball"}
                         };
        }

        public void LoadData(int sportId, string callingPageName)
        {
            CurrentSportId = sportId;
            _callingPageName = callingPageName;

            SportName = ((SportType) Enum.Parse(typeof (SportType), sportId.ToString(), true)).ToString();

            RaisePropertyChanged(() => ShowSportSelector);
        }

        public string SportName
        {
            get { return _sportName; }
            set { _sportName = value; RaisePropertyChanged(() => SportName); }
        }

        public string DrillName
        {
            get { return _drillName; }
            set { _drillName = value; RaisePropertyChanged(() => DrillName); }
        }

        public string DrillNotes
        {
            get { return _drillNotes; }
            set { _drillNotes = value; RaisePropertyChanged(() => DrillNotes); }
        }

        public RelayCommand SaveNewDrillCommand
        {
            get { return _saveNewDrillCommand ?? (_saveNewDrillCommand = new RelayCommand(SaveNewDrill)); }
        }

        private void SaveNewDrill()
        {

            var drill = new PracticeDrillDto
                {
                    SportTypeId = _currentSportId,
                    Name = DrillName,
                    Notes = DrillNotes
                };

            _drillsRepository.Save(drill);

            var newId = drill.Id;

            _sessonStateService.Clear();
            _sessonStateService.Set(SessoinStateConstants.NewDrill, newId);

            _navigationService.GoBack();
        }

        public bool ShowSportSelector
        {
            get { return _currentSportId == 0; }
        }

        public int CurrentSportId
        {
            get { return _currentSportId; }
            set { _currentSportId = value;
            RaisePropertyChanged(() => CurrentSportId);}
        }

        public Sport SelectedSport
        {
            get { return _selectedSport; }
            set
            {
                _selectedSport = value;
                CurrentSportId = value.Id;
                SportName = ((SportType)Enum.Parse(typeof(SportType), value.Id.ToString(), true)).ToString();

                RaisePropertyChanged(() => SelectedSport);
                SaveNewDrillCommand.RaiseCanExecuteChanged();
            }
        }

        public IList<Sport> Sports
        {
            get { return _sports; }
            set
            {
                _sports = value;
                RaisePropertyChanged(() => Sports);
            }
        }
    }

}