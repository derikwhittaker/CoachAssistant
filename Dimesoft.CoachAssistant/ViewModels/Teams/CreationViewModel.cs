using System;
using System.Collections.Generic;
using System.Linq;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;

namespace Dimesoft.CoachAssistant.ViewModels.Teams
{
    public class CreationViewModel : BaseVM
    {
        private readonly INavigationService _navigationService;
        private readonly IEventRepository _eventRepository;
        private IList<Sport> _sports;
        private Sport _selectedSport;
        private string _teamName;
        private RelayCommand _saveTeamCommand;
        private bool _isSaveEnabled;
        private TeamDto _currentTeam = new TeamDto();

        public CreationViewModel(INavigationService navigationService, IEventRepository eventRepository)
        {
            _navigationService = navigationService;
            _eventRepository = eventRepository;

            PageTitle = "Team Maintenance";

            Sports = new List<Sport>
                         {
                             new Sport {Id = (int) SportType.Unknown, Name = "No Selection Made"},
                             new Sport {Id = (int) SportType.Soccer, Name = "Soccer"},
                             new Sport {Id = (int) SportType.Baseball, Name = "Baseball"},
                             new Sport {Id = (int) SportType.Basketball, Name = "Basketball"}
                         };
        }

        public void LoadData( int teamId )
        {
            _currentTeam = _eventRepository.Teams().FirstOrDefault(x => x.Id == teamId) ?? new TeamDto();

            SelectedSport = Sports.FirstOrDefault(x => x.Id == _currentTeam.SportTypeId );

            RaisePropertyChanged(() => TeamName);
            RaisePropertyChanged(() => SelectedSport);
        }

        public RelayCommand SaveTeamCommand
        {
            get { return _saveTeamCommand ?? (_saveTeamCommand = new RelayCommand(SaveTeam, CanSaveTeam)); }
        }

        private bool CanSaveTeam()
        {
            return !string.IsNullOrEmpty(TeamName) && SelectedSport != null && SelectedSport.Id > 0;
        }

        private void SaveTeam()
        {

            _eventRepository.Save(_currentTeam);

            _navigationService.GoBack();
        }
        
        public string TeamName
        {
            get { return _currentTeam.Name; }
            set
            {
                _currentTeam.Name = value; 
                RaisePropertyChanged(() => TeamName);
                SaveTeamCommand.RaiseCanExecuteChanged();
            }
        }

        public Sport SelectedSport
        {
            get { return _selectedSport; }
            set
            {
                _selectedSport = value;
                _currentTeam.SportTypeId = value.Id;
                RaisePropertyChanged(() => SelectedSport);
                SaveTeamCommand.RaiseCanExecuteChanged();
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