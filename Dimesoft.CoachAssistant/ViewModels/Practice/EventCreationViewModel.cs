using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Microsoft.Phone.Reactive;
using EvenTypeEnum = Dimesoft.CoachAssistant.Domain.Models.EventType;
using EventType = Dimesoft.CoachAssistant.Models.EventType;

namespace Dimesoft.CoachAssistant.ViewModels.Practice
{
    public class EventCreationViewModel : BaseVM
    {
        private readonly IEventRepository _eventRepository;
        private IList<EventType> _eventTypes;
        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();
        private Team _selectedTeam;
        private Team _selectedOpponent;
        private EventType _selectedEventType;
        private ObservableCollection<Team> _opponents;
        private bool _showOpponents;
        private DateTime _eventDate = DateTime.Now;
        private DateTime _eventTime = DateTime.Now;

        public EventCreationViewModel( IEventRepository eventRepository )
        {
            _eventRepository = eventRepository;
            PageTitle = "Event Creation";
        }

        public void LoadData()
        {
            IsBusy = true;

            Scheduler.NewThread.Schedule(() =>
                                             {

                                                var sports = new List<Sport>
                                                                     {
                                                                         new Sport {Id = (int) SportType.Unknown, Name = "No Selection Made"},
                                                                         new Sport {Id = (int) SportType.Soccer, Name = "Soccer"},
                                                                         new Sport {Id = (int) SportType.Baseball, Name = "Baseball"},
                                                                         new Sport {Id = (int) SportType.Basketball, Name = "Basketball"}
                                                                     };

                                                 var eventTypes = new List<EventType>
                                                                  {
                                                                      new EventType { Id = (int) EvenTypeEnum.Practice, Name = EvenTypeEnum.Practice.ToString() },
                                                                      new EventType { Id = (int) EvenTypeEnum.Game, Name = EvenTypeEnum.Game.ToString() }
                                                                  };
                                                 
                                                 var teams = _eventRepository.Teams().Select(x =>
                                                                                 {
                                                                                     var sport = sports.FirstOrDefault(s => s.Id == x.SportTypeId);
                                                                                     var team = new Team(x, sport);

                                                                                     return team;
                                                                                 }).ToList();

                                                 HandleDataLoadedCallback(teams, eventTypes);

                                             });
            
        }

        private void HandleDataLoadedCallback(IList<Team> teams, IList<EventType> eventTypes )
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Teams = new ObservableCollection<Team>(teams);
                Opponents = new ObservableCollection<Team>(teams);

                EventTypes = eventTypes;

                IsBusy = false;
            });
        }

        public IList<EventType> EventTypes
        {
            get { return _eventTypes; }
            set
            {
                _eventTypes = value;
                RaisePropertyChanged(() => EventTypes);
            }
        }

        public EventType SelectedEventType
        {
            get { return _selectedEventType; }
            set
            {
                _selectedEventType = value;

                if (value.Id == (int)EvenTypeEnum.Game)
                {
                    ShowOpponents = true;
                }
                else
                {
                    ShowOpponents = false;
                }

                RaisePropertyChanged(() => SelectedEventType);
            }
        }

        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;
                RaisePropertyChanged(() => Teams);
            }
        }

        public ObservableCollection<Team> Opponents
        {
            get { return _opponents; }
            set
            {
                _opponents = value;
                RaisePropertyChanged(() => Opponents);
            }
        }

        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                RaisePropertyChanged(() => SelectedTeam);
            }
        }

        public Team SelectedOpponent
        {
            get { return _selectedOpponent; }
            set
            {
                _selectedOpponent = value;
                RaisePropertyChanged(() => SelectedOpponent);
            }
        }

        public bool ShowOpponents
        {
            get { return _showOpponents; }
            set
            {
                _showOpponents = value;
                RaisePropertyChanged(() => ShowOpponents);
            }
        }

        public DateTime EventDate
        {
            get { return _eventDate; }
            set
            {
                _eventDate = value;
                RaisePropertyChanged(() => EventDate);
            }
        }

        public DateTime EventTime
        {
            get { return _eventTime; }
            set
            {
                _eventTime = value;
                RaisePropertyChanged(() => EventTime);
            }
        }
    }
}
