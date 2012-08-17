using System.Collections.Generic;
using System.Linq;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Practice;

namespace Dimesoft.CoachAssistant.Views.Practice
{
    public partial class DrillListingPage : PageBase
    {
        public int SportId { get; set; }
        public string SelectedDrills { get; set; }

        public DrillListingPage()
        {
            InitializeComponent();

            DataContext = new DrillListingViewModel( new DrillsRepository(), new SessonStateService() );
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs args)
        {
            var drills = new List<int>();

            if (!string.IsNullOrEmpty(SelectedDrills))
            {
                drills = SelectedDrills.Split('|').Select(int.Parse).ToList();
            }

            ViewModel.LoadData(SportId, drills);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SportId = int.Parse(NavigationContext.QueryString[QueryStringConstants.SportId]);
            SelectedDrills = NavigationContext.QueryString[QueryStringConstants.SelectedDrills];

            BackgroundImage.ImageSource = ViewModel.GetBackgroundImage(SportId);
        }

        public DrillListingViewModel ViewModel
        {
            get { return DataContext as DrillListingViewModel; }
        }

        private void SelectItemButtonClick(object sender, System.EventArgs e)
        {
            ViewModel.SelectItemCommand.Execute(null);

            NavigationService.GoBack();
        }

        private void CancelItemButtonClick(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}