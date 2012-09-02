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
        private IList<SportDto> _sports;
        private SportDto _selectedSport;
        private RelayCommand _saveTeamCommand;
        private bool _hasPlayers;
        private TeamDto _currentTeam = new TeamDto();
        private RelayCommand _addPlayerCommand;
        private IList<Player> _players;
        private Player _selectedPlayer;

        public CreationViewModel(INavigationService navigationService, IEventRepository eventRepository)
        {
            _navigationService = navigationService;
            _eventRepository = eventRepository;

            PageTitle = "Team Maintenance";

            Sports = _eventRepository.Sports();
        }

        public void LoadData( int teamId )
        {

            Scheduler.NewThread.Schedule(() =>
                                             {
                                                 IsBusy = true;

                                                 _currentTeam = _eventRepository.Teams().FirstOrDefault(x => x.Id == teamId) ?? new TeamDto();
                                                 SelectedSport = Sports.FirstOrDefault(x => x.Id == _currentTeam.SportTypeId);

                                                 HandleLoadedCallback(_currentTeam, teamId);
                                             });           
        }

        private void HandleLoadedCallback(TeamDto currentTeam, int teamId)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                HasPlayers = currentTeam.Players != null && currentTeam.Players.Any();

                if (currentTeam.Players != null)
                {
                    Players = currentTeam.Players.Select(x => new Player(x)).ToList();    
                }

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
            var url = string.Format("/Views/Teams/PlayerCreationPage.xaml?{0}={1}&{2}={3}", QueryStringConstants.PlayerId, 0, QueryStringConstants.TeamId, _currentTeam.Id);

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }


        private void EditSelectedPlayer(Player value)
        {
            var url = string.Format("/Views/Teams/PlayerCreationPage.xaml?{0}={1}&{2}={3}", QueryStringConstants.PlayerId, value.Dto.Id, QueryStringConstants.TeamId, _currentTeam.Id);

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

        public SportDto SelectedSport
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

        public IList<SportDto> Sports
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

        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;

                RaisePropertyChanged( () => SelectedPlayer);

                if ( value != null )
                {
                    EditSelectedPlayer(value);   
                }
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