using System.Collections.Generic;
using System.Windows.Navigation;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Events;
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

        public GameEventLandingViewModel ViewModel
        {
            get { return DataContext as GameEventLandingViewModel; }
        }
    }
}