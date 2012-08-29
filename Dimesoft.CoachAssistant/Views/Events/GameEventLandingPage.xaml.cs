using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Events;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using NavigationService = Dimesoft.CoachAssistant.Services.NavigationService;

namespace Dimesoft.CoachAssistant.Views.Events
{
    public partial class GameEventLandingPage : PageBase
    {
        public int EventId { get; set; }
        public int SportTypeId { get; set; }
        public bool FromTile { get; set; }
        private ISessonStateService _sessonStateService = new SessonStateService();

        public GameEventLandingPage()
        {
            InitializeComponent();

            this.SetValue(RadSlideContinuumAnimation.ApplicationHeaderElementProperty, this.PageTitle);
            this.SetValue(RadSlideContinuumAnimation.HeaderElementProperty, this.PageTitle);

            var vm = new GameEventLandingViewModel( new EventRepository(), new DrillsRepository(), new NavigationService(), new TileService() );


            vm.PropertyChanged += (s, e) =>
                                      {
                                          if (e.PropertyName == "GameEvent")
                                          {
                                              SetGameStateMenuState();
                                              SetPinStateMenuState();
                                          }
                                          else if (e.PropertyName == "IsEventPinned")
                                          {
                                              SetPinStateMenuState();
                                          }
                                      };

            DataContext = vm;
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs args)
        {
            _sessonStateService.Clear();

            ViewModel.LoadData(EventId);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

//            FromTile = !string.IsNullOrWhiteSpace(NavigationContext.TryGetQueryString(QueryStringConstants.FromTile));
            EventId = NavigationContext.TryGetQueryInt(QueryStringConstants.EventId);
            SportTypeId = NavigationContext.TryGetQueryInt(QueryStringConstants.SportTypeId);

            PanoramaBackground.ImageSource = ViewModel.GetBackgroundImage(SportTypeId);
        }


        private void SetPinStateMenuState()
        {
            var appBar = (ApplicationBarMenuItem)this.ApplicationBar.MenuItems[1];
            if (ViewModel.IsEventPinned)
            {

                appBar.Text = "Unpin Event";
            }
            else
            {
                appBar.Text = "Pin Event";
            }
        }

        private void SetGameStateMenuState()
        {

            var appBar = (ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0];
            if (ViewModel.GameEvent.IsCompleted)
            {

                appBar.Text = "Mark as Active";
            }
            else
            {
                appBar.Text = "Mark as Compledted";
            }
        }

        public GameEventLandingViewModel ViewModel
        {
            get { return DataContext as GameEventLandingViewModel; }
        }

        private void GameCompletedClicked(object sender, EventArgs e)
        {
            ViewModel.ToggleEventStateCommand.Execute(null);
        }

        private void PinEventClicked(object sender, EventArgs e)
        {
            ViewModel.TogglePinStateForEventCommand.Execute(null);
        }

        private void ApplicationBar_StateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {
            SetGameStateMenuState();
        }
    }
}