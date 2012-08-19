using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using GalaSoft.MvvmLight.Command;
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
        private ObservableCollection<Field> _fields;
        private Field _selectedField;
        private Models.Event _currentEvent = new Models.Event(new EventDto());
        private RelayCommand _saveEventCommand;
        private IList<Sport> _sports = new List<Sport>();

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

                                                 _sports = new List<Sport>
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
                                                                                     var sport = _sports.FirstOrDefault(s => s.Id == x.SportTypeId);
                                                                                     var team = new Team(x, sport);

                                                                                     return team;
                                                                                 }).ToList();

                                                 var fields = _eventRepository.Locations().Select(x => new Field(x)).ToList();

                                                 HandleDataLoadedCallback(teams, fields, eventTypes);

                                             });
            
        }

        private void HandleDataLoadedCallback(IList<Team> teams, IList<Field> fields, IList<EventType> eventTypes )
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Teams = new ObservableCollection<Team>(teams);
                Opponents = new ObservableCollection<Team>(teams);
                Fields = new ObservableCollection<Field>(fields);

                EventTypes = eventTypes;

                IsBusy = false;
            });
        }

        public RelayCommand SaveEventCommand
        {
            get { return _saveEventCommand ?? (_saveEventCommand = new RelayCommand(SaveEvent)); }
        }

        private void SaveEvent()
        {
            var newEvent = new EventDto
                {
                    EventTypeId = SelectedEventType.Id,
                    SportTypeId = SelectedTeam.Dto.SportTypeId,
                    Team = SelectedTeam.Dto,
                    Opponent = SelectedOpponent != null ? SelectedOpponent.Dto : null,
                    Location = SelectedField.Dto,
                    Date = new DateTime(EventDate.Year, EventDate.Month, EventDate.Day, EventTime.Hour, EventTime.Minute, 0),
                    Notes = this.Notes,
                };

            _eventRepository.Save(newEvent);
        }

        public Models.Event CurrentEvent
        {
            get { return _currentEvent; }
            set { _currentEvent = value; }
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

                CurrentEvent.EventType = (EvenTypeEnum)value.Id;

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

        public ObservableCollection<Field> Fields
        {
            get { return _fields; }
            set
            {
                _fields = value;
                RaisePropertyChanged(() => Fields);
            }
        }

        public Field SelectedField
        {
            get { return _selectedField; }
            set
            {
                _selectedField = value;                
                RaisePropertyChanged(() => SelectedField);
            }
        }

        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                RaisePropertyChanged(() => SelectedTeam);
                RaisePropertyChanged(() => SportName);
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

        public string Notes { get; set; }

        public string SportName
        {
            get
            {
                if (SelectedTeam == null)
                {
                    return "";
                }
                else
                {
                    return SelectedTeam.Name;
                }
            }
        }
    }
}
