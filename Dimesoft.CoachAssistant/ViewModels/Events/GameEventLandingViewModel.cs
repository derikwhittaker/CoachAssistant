using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Dimesoft.CoachAssistant.ViewModels.Events
{
    public class GameEventLandingViewModel : BaseVM
    {

        private RelayCommand _pinEventCommand;
        private Team _team;
        private bool _hasPlayers;
        private IList<Player> _players;
        private readonly IEventRepository _eventRepository;
        private readonly IDrillsRepository _drillsRepository;
        private readonly INavigationService _navigationService;
        private readonly ITileService _tileService;

        public GameEventLandingViewModel(IEventRepository eventRepository, IDrillsRepository drillsRepository,
            INavigationService navigationService, ITileService tileService)
        {
            _eventRepository = eventRepository;
            _drillsRepository = drillsRepository;
            _navigationService = navigationService;
            _tileService = tileService;
        }

        public void LoadData(int eventId)
        {
            if (DataLoaded) { return; }

            Scheduler.NewThread.Schedule(() =>
            {

                var gameDto = _eventRepository.Event(eventId);
                var teamDto = _eventRepository.Teams().FirstOrDefault(x => x.Id == gameDto.Team.Id);
                var sportDto = _eventRepository.Sports().FirstOrDefault(x => x.Id == teamDto.SportTypeId);

                var gameEvent = new GameEvent(gameDto);
                var team = new Team(teamDto, sportDto);

                HandleLoadDataCallback(gameEvent, team);
            });
        }

        private void HandleLoadDataCallback(GameEvent gameEvent, Team team)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              GameEvent = gameEvent;
                                                              Team = team;

                                                              HasPlayers = Team.Players != null && Team.Players.Any();

                                                              if (Team.Players != null)
                                                              {
                                                                  Players = Team.Players.ToList();
                                                              }

                                                              DataLoaded = true;
                                                              IsBusy = false;
                                                          });
        }

        private GameEvent _event;
        public GameEvent GameEvent
        {
            get { return _event; }
            set
            {
                _event = value;

                RaisePropertyChanged(() => GameEvent);
            }
        }

        public Team Team
        {
            get { return _team; }
            set { _team = value; }
        }

        private RelayCommand _toggleEventStateCommand;
        public RelayCommand ToggleEventStateCommand
        {
            get { return _toggleEventStateCommand ?? (_toggleEventStateCommand = new RelayCommand(ToggleEventState)); }
        }

        private void ToggleEventState()
        {
            GameEvent.IsCompleted = !GameEvent.IsCompleted;

            _eventRepository.Save(GameEvent.Dto);
        }
        
        public RelayCommand TogglePinStateForEventCommand
        {
            get { return _pinEventCommand ?? (_pinEventCommand = new RelayCommand(TogglePinStateForEvent)); }
        }

        private void TogglePinStateForEvent()
        {
            if (_tileService.SecondaryEvenTileExists(GameEvent.SportType, GameEvent.Id))
            {
                _tileService.DeleteSecondaryTile(GameEvent.SportType, GameEvent.Id);
            }
            else
            {
                _tileService.CreateEventTile(GameEvent.SportType, GameEvent.EventType, GameEvent.Id,
                                             GameEvent.Dto.Date, GameEvent.LocationName, GameEvent.TeamName,
                                             GameEvent.OpponentTeamName);
            }

            RaisePropertyChanged(() => IsEventPinned);
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

        public bool IsEventPinned
        {
            get
            {
                return _tileService.SecondaryEvenTileExists(GameEvent.SportType, GameEvent.Id);
            }
        }
    }
}
