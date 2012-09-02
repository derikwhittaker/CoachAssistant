using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Dimesoft.CoachAssistant.ViewModels.Teams
{
    public class CreationViewModel : BaseVM
    {
        private readonly INavigationService _navigationService;
        private readonly IEventRepository _eventRepository;
        private IList<Sport> _sports;
        private Sport _selectedSport;
        private RelayCommand _saveTeamCommand;
        private bool _hasPlayers;
        private TeamDto _currentTeam = new TeamDto();
        private RelayCommand _addPlayerCommand;
        private IList<Player> _players;

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

            Scheduler.NewThread.Schedule(() =>
                                             {
                                                 IsBusy = true;

                                                 _currentTeam = _eventRepository.Teams().FirstOrDefault(x => x.Id == teamId) ?? new TeamDto();

                                                 HandleLoadedCallback(_currentTeam, teamId);
                                             });           
        }

        private void HandleLoadedCallback(TeamDto currentTeam, int teamId)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _currentTeam = _eventRepository.Teams().FirstOrDefault(x => x.Id == teamId) ?? new TeamDto();
                HasPlayers = _currentTeam.Players != null && _currentTeam.Players.Any();

                if (_currentTeam.Players != null)
                {
                    Players = _currentTeam.Players.Select(x => new Player(x)).ToList();    
                }

                SelectedSport = Sports.FirstOrDefault(x => x.Id == _currentTeam.SportTypeId);

                RaisePropertyChanged(() => SelectedSport);

                RaisePropertyChanged(() => TeamName);
                RaisePropertyChanged(() => SelectedSport); 

                IsBusy = false;
            });

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

        public RelayCommand AddPlayerCommand
        {
            get { return _addPlayerCommand ?? (_addPlayerCommand = new RelayCommand(AddPlayer)); }
        }

        private void AddPlayer()
        {
            var url = string.Format("/Views/Teams/PlayerCreationPage.xaml?{0}=0&{1}={2}", QueryStringConstants.PlayerId, QueryStringConstants.TeamId, _currentTeam.Id);

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
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

        public bool MyTeam
        {
            get { return _currentTeam.MyTeam; }
            set
            {
                _currentTeam.MyTeam = value;
                RaisePropertyChanged(() => MyTeam);
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

        public IList<Player> Players
        {
            get { return _players; }
            set
            {
                _players = value;

                RaisePropertyChanged(() => Players);
            }
        }

        public bool HasPlayers
        {
            get { return _hasPlayers; }
            set
            {
                _hasPlayers = value;
                RaisePropertyChanged(() => HasPlayers);
            }
        }
    }
}