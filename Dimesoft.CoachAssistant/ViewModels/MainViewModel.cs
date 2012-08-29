using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;
using Event = Dimesoft.CoachAssistant.Models.Event;

namespace Dimesoft.CoachAssistant.ViewModels
{
    public class MainViewModel : BaseVM
    {
        private readonly INavigationService _navigationService;
        private Event _selectedEvent;
        private DashboardViewState _dashboardViewState = DashboardViewState.ShowActive; 
        private ObservableCollection<Event> _events;
        private RelayCommand _teamListingCommand;
        private RelayCommand _fieldListingCommand;
        private RelayCommand _createEventCommand;
        private RelayCommand _drillsListingCommand;

        public MainViewModel(){}

        public MainViewModel(INavigationService navigationService)
            : this()
        {
            _navigationService = navigationService;
        }

        public void LoadData()
        {
            IsBusy = true;

            Scheduler.NewThread.Schedule(() =>
                                             {
                                                 SetupDatabase();

                                                 var repo = new EventRepository();
                                                 IList<EventDto> eventDtos = new List<EventDto>();
                                                 switch (DashboardViewState)
                                                 {
                                                    case DashboardViewState.ShowAll:
                                                    eventDtos = repo.All();
                                                    break;

                                                    case DashboardViewState.ShowActive:
                                                    eventDtos = repo.Open();
                                                    break;

                                                    case DashboardViewState.ShowCompleted:
                                                    eventDtos = repo.Completed();
                                                    break;
                                                 }

                                                 var events = new ObservableCollection<Event>(eventDtos.Select(x => new Event(x)).ToList());

                                                 HandleLoadDataCallback(events);
                                             });
        }

        private void SetupDatabase()
        {
            var drillsRepository = new DrillsRepository();
            var eventsRepository = new EventRepository();

            drillsRepository.SetupInitialData();            
            eventsRepository.SetupInitialData();

        }

        private void HandleLoadDataCallback(ObservableCollection<Event> events)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              Events = events;

                                                              DataLoaded = true;
                                                              IsBusy = false;
                                                          });
        }

        public RelayCommand CreateEventCommand
        {
            get { return _createEventCommand ?? (_createEventCommand = new RelayCommand(CreateEvent)); }
        }

        private void CreateEvent()
        {
            var url = string.Format("/Views/Events/EventCreationPage.xaml");

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        public RelayCommand DrillsListingCommand
        {
            get { return _drillsListingCommand ?? (_drillsListingCommand = new RelayCommand(DrillsListing)); }
        }

        private void DrillsListing()
        {
            var url = string.Format("/Views/Events/DrillsListingPanoramaPage.xaml");

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));            
        }

        public RelayCommand FieldListingCommand
        {
            get { return _fieldListingCommand ?? (_fieldListingCommand = new RelayCommand(FieldListing)); }
        }

        private void FieldListing()
        {
            var url = string.Format("/Views/Fields/ListingPage.xaml");

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }
        
        public RelayCommand TeamListingCommand
        {
            get { return _teamListingCommand ?? (_teamListingCommand = new RelayCommand(TeamListing)); }
        }

        private void TeamListing()
        {
            var url = string.Format("/Views/Teams/ListingPage.xaml");

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }
        
        public ObservableCollection<Event> Events
        {
            get { return _events; }
            set
            {
                _events = value; 
                RaisePropertyChanged(() => Events);
                RaisePropertyChanged(() => EventsCount);
            }
        }

        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;

                RaisePropertyChanged(() => SelectedEvent);

                if (_selectedEvent != null)
                {
                    var url = "";
                    if ( _selectedEvent.EventType == EventType.Practice )
                    {
                         url = string.Format("/Views/Events/PracticeEventLandingPage.xaml?{0}={1}&{2}={3}",
                                QueryStringConstants.EventId, SelectedEvent.Id, QueryStringConstants.SportTypeId, (int)SelectedEvent.SportType);
                    }
                    else if (_selectedEvent.EventType == EventType.Game )
                    {
                        url = string.Format("/Views/Events/GameEventLandingPage.xaml?{0}={1}&{2}={3}",
                            QueryStringConstants.EventId, SelectedEvent.Id, QueryStringConstants.SportTypeId, (int)SelectedEvent.SportType);                        
                    }

                    _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
                }


            }
        }

        public string EventsCount
        {
            get
            {
                if (Events == null) { return ""; }


                return string.Format("{0} Events", Events.Count);
            }
        }

        public DashboardViewState DashboardViewState
        {
            get { return _dashboardViewState; }
            set
            {
                if (_dashboardViewState != value)
                {
                    _dashboardViewState = value;
                    LoadData();
                    RaisePropertyChanged(() => DashboardViewState);
                }
            }
        }
    }
}