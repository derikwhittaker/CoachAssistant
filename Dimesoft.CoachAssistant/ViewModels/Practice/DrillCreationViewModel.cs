using System;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;

namespace Dimesoft.CoachAssistant.ViewModels.Practice
{
    public class DrillCreationViewModel : BaseVM
    {
        private int _currentSportId = 0;
        private string _callingPageName;
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
        }

        public void LoadData(int sportId, string callingPageName)
        {
            _currentSportId = sportId;
            _callingPageName = callingPageName;

            SportName = ((SportType) Enum.Parse(typeof (SportType), sportId.ToString(), true)).ToString();
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
    }

}