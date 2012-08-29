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

                var asDto = _eventRepository.Event(eventId);

                //var allDrills = _drillsRepository.ForSport(asDto.SportType);
                //var foundDrill = allDrills.FirstOrDefault(x => x.Id == newDrillId);
                //asDto.PracticeDrills.Add(foundDrill);

                //var practiceEvent = new PracticeEvent(asDto);
                var gameEvent = new GameEvent(asDto);

                //_eventRepository.Save(asDto);

                HandleLoadDataCallback(gameEvent);
            });
        }

        private void HandleLoadDataCallback(GameEvent gameEvent)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              GameEvent = gameEvent;

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

        private RelayCommand _pinEventCommand;
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

        public bool IsEventPinned
        {
            get
            {
                return _tileService.SecondaryEvenTileExists(GameEvent.SportType, GameEvent.Id);
            }
        }
    }
}
