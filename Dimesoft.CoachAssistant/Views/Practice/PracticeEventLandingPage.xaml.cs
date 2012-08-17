using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Practice;
using Telerik.Windows.Controls;
using NavigationService = Dimesoft.CoachAssistant.Services.NavigationService;

namespace Dimesoft.CoachAssistant.Views.Practice
{
    public partial class PracticeEventLandingPage : PageBase
    {
        public int EventId { get; set; }
        public int SportTypeId { get; set; }

        private ISessonStateService _sessonStateService = new SessonStateService();
        public PracticeEventLandingPage() : base()
        {
            InitializeComponent();

            this.SetValue(RadSlideContinuumAnimation.ApplicationHeaderElementProperty, this.PageTitle);
            this.SetValue(RadSlideContinuumAnimation.HeaderElementProperty, this.PageTitle);
            
            DataContext = new EventLandingViewModel(new EventRepository(), new DrillsRepository(),  new NavigationService());
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

            EventId = NavigationContext.TryGetQueryInt(QueryStringConstants.EventId);
            SportTypeId = NavigationContext.TryGetQueryInt(QueryStringConstants.SportTypeId);

            PanoramaBackground.ImageSource = ViewModel.GetBackgroundImage(SportTypeId);
        }

        public EventLandingViewModel ViewModel
        {
            get { return DataContext as EventLandingViewModel; }
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