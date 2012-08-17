using System;
using System.Collections.Generic;
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
            var team = new TeamDto
                {
                    Name = TeamName,
                    SportTypeId = SelectedSport.Id
                };

            _eventRepository.Save(team);

            _navigationService.GoBack();
        }
        
        public string TeamName
        {
            get { return _teamName; }
            set
            {
                _teamName = value; 
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