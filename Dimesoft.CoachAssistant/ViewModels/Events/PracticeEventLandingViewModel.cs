using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;
using System.Linq;

namespace Dimesoft.CoachAssistant.ViewModels.Events
{
    public class PracticeEventLandingViewModel : BaseVM
    {

        private readonly IEventRepository _eventRepository;
        private readonly IDrillsRepository _drillsRepository;
        private readonly INavigationService _navigationService;
        private readonly ITileService _tileService;

        // for sample data only
        public PracticeEventLandingViewModel() { }

        public PracticeEventLandingViewModel(IEventRepository eventRepository, IDrillsRepository drillsRepository, 
            INavigationService navigationService, ITileService tileService) : this()
        {
            _eventRepository = eventRepository;
            _drillsRepository = drillsRepository;
            _navigationService = navigationService;
            _tileService = tileService;
        }

        public void LoadData(int eventId)
        {
            if (DataLoaded) { return; }

            LoadData(eventId, new List<int>());
        }

        public void LoadData(int eventId, int newDrillId )
        {
            IsBusy = true;

            Scheduler.NewThread.Schedule(() =>
                                             {

                                                 var asDto = _eventRepository.PracticeEvent(eventId);

                                                 var allDrills = _drillsRepository.ForSport(asDto.SportType);
                                                 var foundDrill = allDrills.FirstOrDefault(x => x.Id == newDrillId);
                                                 asDto.PracticeDrills.Add(foundDrill);

                                                 var practiceEvent = new PracticeEvent(asDto);

                                                 _eventRepository.Save(asDto);

                                                 HandleLoadDataCallback(practiceEvent);
                                             });
        }

        public void LoadData(int eventId, IList<int> drills)
        {
            IsBusy = true;

            Scheduler.NewThread.Schedule(() =>
                                             {
                                                 Debug.WriteLine(string.Format("Load data - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

                                                 var asDto = _eventRepository.PracticeEvent(eventId);
                                                 
                                                 if (drills.Any())
                                                 {
                                                     var allDrills = _drillsRepository.ForSport(asDto.SportType);
                                                     asDto.PracticeDrills = new List<PracticeDrillDto>();

                                                     foreach (var drill in drills)
                                                     {
                                                         var foundDrill = allDrills.FirstOrDefault(x => x.Id == drill);

                                                         asDto.PracticeDrills.Add(foundDrill);
                                                     }
                                                 }

                                                 var practiceEvent = new PracticeEvent(asDto);

                                                 _eventRepository.Save(asDto);

                                                 HandleLoadDataCallback(practiceEvent);
                                             });            
        }

        private void HandleLoadDataCallback(PracticeEvent practiceEvent)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              PracticeEvent = practiceEvent;

                                                              DataLoaded = true;
                                                              IsBusy = false;
                                                          });
        }

        public override BitmapImage GetBackgroundImage(int sportId)
        {
            var path = "";
            switch ((SportType)sportId)
            {
                case SportType.Baseball:
                    path = "../../Images/BaseballBackground_1024x768.jpg";
                    break;

                case SportType.Basketball:
                    path = "../../Images/BasketballBackground_1024x768.jpg";
                    break;

                case SportType.Soccer:
                    path = "../../Images/SoccerBackground_1024x768.jpg";
                    break;

                default:
                    path = "../../Images/SoccerBackground_1024x768.jpg";
                    break;
            }

            var bitmap = new BitmapImage
                             {
                                 CreateOptions = BitmapCreateOptions.BackgroundCreation,
                                 UriSource = new Uri(path, UriKind.RelativeOrAbsolute),
                             };

            return bitmap;
        }

        private string GetSelectExistingPracticeTileImage()
        {
            switch (PracticeEvent.SportType)
            {
                case SportType.Baseball:
                    return "../../Images/Tiles/SelectExistingBaseballPracticeTile_132x276.jpg";

                case SportType.Basketball:
                    return "../../Images/Tiles/SelectExistingBasketballPracticeTile_132x276.jpg";

                case SportType.Soccer:
                    return "../../Images/Tiles/SelectExistingSoccerPracticeTile_132x276.jpg";

                default:
                    return "../../Images/Tiles/SelectExistingSoccerPracticeTile_132x276.jpg";
            }
        }

        private string GetCreateNewPracticeTileImage()
        {
            switch (PracticeEvent.SportType)
            {
                case SportType.Baseball:
                    return "../../Images/Tiles/CreateNewBaseballPracticeTile_132x276.jpg";

                case SportType.Basketball:
                    return "../../Images/Tiles/CreateNewBaseketballPracticeTile_132x276.jpg";

                case SportType.Soccer:
                    return "../../Images/Tiles/CreateNewSoccerPracticeTile_132x276.jpg";

                default:
                    return "../../Images/Tiles/CreateNewSoccerPracticeTile_132x276.jpg";
            }
        }

        private RelayCommand _selectExistingDrillCommand;
        public RelayCommand SelectExistingDrillCommand
        {
            get { return _selectExistingDrillCommand ?? (_selectExistingDrillCommand = new RelayCommand(SelectExistingDrill)); }
        }

        private void SelectExistingDrill()
        {
            var currentDrills = PracticeEvent.PracticeDrills.Select(x => x.Id).ToArray();

            var url = string.Format("/Views/Events/DrillListingPage.xaml?{0}={1}&{2}={3}",
                QueryStringConstants.SportId, (int)PracticeEvent.SportType, QueryStringConstants.SelectedDrills, string.Join("|", currentDrills));

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        private RelayCommand _createNewDrillCommand;
        public RelayCommand CreateNewDrillCommand
        {
            get { return _createNewDrillCommand ?? (_createNewDrillCommand = new RelayCommand(CreateNewDrill)); }
        }

        private void CreateNewDrill()
        {
            var url = string.Format("/Views/Events/DrillCreationPage.xaml?{0}={1}&{2}={3}", 
                QueryStringConstants.SportId, (int)PracticeEvent.SportType,
                QueryStringConstants.CallingPageName, RoutingPageConstants.PracticeEventLandingPage);

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        public RelayCommand ToggleEventStateCommand
        {
            get { return _toggleEventStateCommand ?? (_toggleEventStateCommand = new RelayCommand(ToggleEventState)); }
        }

        private void ToggleEventState()
        {
            PracticeEvent.IsCompleted = !PracticeEvent.IsCompleted;
            
            _eventRepository.Save(PracticeEvent.Dto);
        }

        public RelayCommand TogglePinStateForEventCommand
        {
            get { return _pinEventCommand ?? (_pinEventCommand = new RelayCommand(TogglePinStateForEvent)); }
        }

        private void TogglePinStateForEvent()
        {
            if (_tileService.SecondaryEvenTileExists(PracticeEvent.SportType, PracticeEvent.Id))
            {
                _tileService.DeleteSecondaryTile(PracticeEvent.SportType, PracticeEvent.Id);
            }
            else
            {
                _tileService.CreateEventTile(PracticeEvent.SportType, PracticeEvent.EventType, PracticeEvent.Id,
                                             PracticeEvent.Dto.Date, PracticeEvent.LocationName, PracticeEvent.TeamName,
                                             PracticeEvent.OpponentTeamName);
            }

            RaisePropertyChanged(() => IsEventPinned);
        }

        public BitmapImage CreateNewPracticeTileImage
        {
            get
            {
                if (PracticeEvent == null) { return null; }

                var bitmap = new BitmapImage
                                 {
                                     CreateOptions = BitmapCreateOptions.BackgroundCreation,
                                     UriSource = new Uri(GetCreateNewPracticeTileImage(), UriKind.RelativeOrAbsolute),
                                 };

                return bitmap;
            }
        }

        public BitmapImage SelectExistingPracticeTileImage
        {
            get
            {
                if (PracticeEvent == null) { return null; }

                var bitmap = new BitmapImage
                                 {
                                     CreateOptions = BitmapCreateOptions.BackgroundCreation,
                                     UriSource = new Uri(GetSelectExistingPracticeTileImage(), UriKind.RelativeOrAbsolute),
                                 };

                return bitmap;
            }
        }

        private PracticeEvent _event;
        public PracticeEvent PracticeEvent
        {
            get { return _event; }
            set
            {
                _event = value;
                
                RaisePropertyChanged(() => PracticeEvent);
                RaisePropertyChanged(() => DrillCount);
                RaisePropertyChanged(() => HasPracticeDrills);
                RaisePropertyChanged(() => SelectExistingPracticeTileImage);
                RaisePropertyChanged(() => CreateNewPracticeTileImage);
            }
        }

        private PracticeDrill _selectedPracticeDrill;
        private bool _showSelectionCheckBoxes = false;
        private RelayCommand _toggleEventStateCommand;
        private RelayCommand _pinEventCommand;


        public PracticeDrill SelectedPracticeDrill
        {
            get { return _selectedPracticeDrill; }
            set
            {
                _selectedPracticeDrill = value;

                if ( !ShowSelectionCheckBoxes &&  PracticeEvent.PracticeDrills.All(x => x.Selected == false))
                {
                    ShowSelectionCheckBoxes = true;
                }
            }
        }

        public string DrillCount 
        {
            get
            {
                if (PracticeEvent == null || PracticeEvent.PracticeDrills == null)
                {
                    return "";
                }
                
                return string.Format("{0} Drills", PracticeEvent.PracticeDrills.Count);
            }
        }

        public bool HasPracticeDrills
        {
            get
            {
                // if we are busy loading data return true in order to hide the text
                if (IsBusy) { return true; }

                if (PracticeEvent == null || PracticeEvent.PracticeDrills == null)
                {
                    return false;
                }

                return PracticeEvent.PracticeDrills.Count > 0;
            }
        }

        public bool ShowSelectionCheckBoxes
        {
            get { return _showSelectionCheckBoxes; }
            set { _showSelectionCheckBoxes = value; RaisePropertyChanged(() => ShowSelectionCheckBoxes); }
        }

        public bool IsEventPinned
        {
            get
            {
                return _tileService.SecondaryEvenTileExists(PracticeEvent.SportType, PracticeEvent.Id);
            }
        }
        
    }
}
