using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;
using Event = Dimesoft.CoachAssistant.Models.Event;


namespace Dimesoft.CoachAssistant
{
    public class MainViewModel : BaseVM
    {
        private readonly INavigationService _navigationService;

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

                                                 var repo = new Domain.Repositories.EventRepository();
                                                 var eventDtos = repo.All();

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

        private RelayCommand _teamListingCommand;
        public RelayCommand TeamListingCommand
        {
            get { return _teamListingCommand ?? (_teamListingCommand = new RelayCommand(TeamListing)); }
        }

        private void TeamListing()
        {
            var url = string.Format("/Views/Teams/ListingPage.xaml");

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        private ObservableCollection<Event> _events;
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
        
        private Event _selectedEvent;
        
        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;

                RaisePropertyChanged(() => SelectedEvent);

                if ( _selectedEvent != null && _selectedEvent.EventType == EventType.Practice)
                {
                    var url = string.Format("/Views/Practice/EventLandingPage.xaml?{0}={1}&{2}={3}", 
                        QueryStringConstants.EventId, SelectedEvent.Id, QueryStringConstants.SportTypeId, (int)SelectedEvent.SportType);

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

    }
}