﻿using System;
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
    public partial class PracticeEventLandingPage : PageBase
    {
        public int EventId { get; set; }
        public int SportTypeId { get; set; }
        public bool FromTile { get; set; }

        private ISessonStateService _sessonStateService = new SessonStateService();
        public PracticeEventLandingPage() : base()
        {
            InitializeComponent();

            this.SetValue(RadSlideContinuumAnimation.ApplicationHeaderElementProperty, this.PageTitle);
            this.SetValue(RadSlideContinuumAnimation.HeaderElementProperty, this.PageTitle);
            
            var vm = new PracticeEventLandingViewModel(new EventRepository(), new DrillsRepository(),  new NavigationService(), new TileService());

            vm.PropertyChanged += (s, e) =>
                                      {
                                          if (e.PropertyName == "PracticeEvent")
                                          {
                                              SetPracticeStateMenuState();
                                              SetPinStateMenuState();
                                          }
                                          else if (e.PropertyName == "IsEventPinned")
                                          {
                                              SetPinStateMenuState();
                                          }
                                      };

            DataContext = vm;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if ( FromTile )
            {
                e.Cancel = true;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                base.OnBackKeyPress(e);    
            }
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

        private void SetPracticeStateMenuState()
        {
        
            var appBar = (ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0];
            if (ViewModel.PracticeEvent.IsCompleted)
            {
                
                appBar.Text = "Mark as Active";
            }
            else
            {
                appBar.Text = "Mark as Compledted";
            }
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs args)
        {
            var drills = _sessonStateService.TryGet(SessoinStateConstants.SelectedDrills);
            var newDrillId = _sessonStateService.TryGet(SessoinStateConstants.NewDrill);

            _sessonStateService.Clear();
            
            if (drills != null)
            {
                var drillsAsList = (List<int>)drills;

                ViewModel.LoadData(EventId, drillsAsList);
            }
            else if (newDrillId != null)
            {
                ViewModel.LoadData(EventId, int.Parse(newDrillId.ToString()));
            }
            else
            {
                ViewModel.LoadData(EventId);
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            FromTile = !string.IsNullOrWhiteSpace( NavigationContext.TryGetQueryString(QueryStringConstants.FromTile) );
            EventId = NavigationContext.TryGetQueryInt(QueryStringConstants.EventId);
            SportTypeId = NavigationContext.TryGetQueryInt(QueryStringConstants.SportTypeId);

            PanoramaBackground.ImageSource = ViewModel.GetBackgroundImage(SportTypeId);            
        }

        private void PracticeCompletedClicked(object sender, EventArgs e)
        {
            ViewModel.ToggleEventStateCommand.Execute(null);
        }

        private void PinEventClicked(object sender, EventArgs e)
        {
            ViewModel.TogglePinStateForEventCommand.Execute(null);
        }

        public PracticeEventLandingViewModel ViewModel
        {
            get { return DataContext as PracticeEventLandingViewModel; }
        }

        private void ApplicationBar_StateChanged(object sender, Microsoft.Phone.Shell.ApplicationBarStateChangedEventArgs e)
        {
            SetPracticeStateMenuState();
        }


    }

    public static class NavigationContextExtensions
    {
        public static string TryGetQueryString( this NavigationContext context, string key )
        {
            try
            {
                return context.QueryString[key];
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static int TryGetQueryInt(this NavigationContext context, string key)
        {
            try
            {
                return int.Parse( context.QueryString[key] );
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}