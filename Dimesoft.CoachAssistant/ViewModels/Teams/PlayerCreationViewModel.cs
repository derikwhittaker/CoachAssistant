using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Dimesoft.CoachAssistant.ViewModels.Teams
{
    public class PlayerCreationViewModel : BaseVM
    {
        private readonly INavigationService _navigationService;
        private readonly IEventRepository _eventRepository;
        private PlayerDto _currentPlayer = new PlayerDto();
        private TeamDto _currentTeam = new TeamDto();
        private RelayCommand _savePlayerCommand;

        public PlayerCreationViewModel(INavigationService navigationService, IEventRepository eventRepository)
        {
            _navigationService = navigationService;
            _eventRepository = eventRepository;

            PageTitle = "Player Maintenance";
        }

        public void LoadData(int teamId, int playerId)
        {
            IsBusy = true;

            Scheduler.NewThread.Schedule(() =>
            {
                _currentTeam = _eventRepository.Teams().FirstOrDefault(x => x.Id == teamId) ?? new TeamDto();
                _currentPlayer = _eventRepository.Players().FirstOrDefault(x => x.Id == playerId) ?? new PlayerDto();

                HandleLoadedCallback();
            });           
        }

        private void HandleLoadedCallback()
        {

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              RaisePropertyChanged(() => FirstName);
                                                              RaisePropertyChanged(() => LastName);
                                                              RaisePropertyChanged(() => Nickname);
                                                              RaisePropertyChanged(() => JerseyNumber);
                                                              RaisePropertyChanged(() => DateOfBirth);

                                                              IsBusy = false;
                                                          });
        }

        public RelayCommand SavePlayerCommand
        {
            get { return _savePlayerCommand ?? (_savePlayerCommand = new RelayCommand(SavePlayer, CanSavePlayer)); }
        }

        private bool CanSavePlayer()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName);

        }

        private void SavePlayer()
        {
            _eventRepository.Save(_currentPlayer);

            var foundPlayer = _currentTeam.Players.FirstOrDefault(x => x.Id == _currentPlayer.Id);

            if (foundPlayer == null)
            {
                _currentTeam.Players.Add(_currentPlayer);
            }
            else
            {
                _currentTeam.Players.Remove(foundPlayer);
                _currentTeam.Players.Add(_currentPlayer);
            }

            _eventRepository.Save(_currentTeam);

            _navigationService.GoBack();
        }

        public string FirstName
        {
            get { return _currentPlayer.FirstName; }
            set { 
                _currentPlayer.FirstName = value;

                RaisePropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get { return _currentPlayer.LastName; }
            set
            {
                _currentPlayer.LastName = value;

                RaisePropertyChanged(() => LastName);
            }
        }

        public string Nickname
        {
            get { return _currentPlayer.Nickname; }
            set
            {
                _currentPlayer.Nickname = value;

                RaisePropertyChanged(() => Nickname);
            }
        }

        public int JerseyNumber
        {
            get { return _currentPlayer.JerseyNumber; }
            set
            {
                _currentPlayer.JerseyNumber = value;

                RaisePropertyChanged(() => JerseyNumber);
            }
        }

        public DateTime? DateOfBirth
        {
            get
            {
                if ( _currentPlayer.DateOfBirth == DateTime.MinValue)
                {
                    return null;
                }

                return _currentPlayer.DateOfBirth;
            }
            set
            {
                _currentPlayer.DateOfBirth = value.Value;

                RaisePropertyChanged(() => DateOfBirth);
            }
        }
    }
}
