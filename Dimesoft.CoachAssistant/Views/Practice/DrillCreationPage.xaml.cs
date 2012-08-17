using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Practice;

namespace Dimesoft.CoachAssistant.Views.Practice
{
    public partial class DrillCreationPage : PageBase
    {
        public int SportId { get; set; }
        public string CallingPageName { get; set; }

        public DrillCreationPage()
        {
            InitializeComponent();

            DataContext = new DrillCreationViewModel(new DrillsRepository(), new SessonStateService(), new NavigationService());
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs args)
        {
            ViewModel.LoadData(SportId, CallingPageName);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SportId = int.Parse(NavigationContext.QueryString[QueryStringConstants.SportId]);
            CallingPageName = NavigationContext.QueryString[QueryStringConstants.CallingPageName];

            BackgroundImage.ImageSource = ViewModel.GetBackgroundImage(SportId);
        }

        public DrillCreationViewModel ViewModel
        {
            get { return DataContext as DrillCreationViewModel; }
        }

        private void SaveNewDrillClicked(object sender, System.EventArgs e)
        {
            ViewModel.SaveNewDrillCommand.Execute(null);
        }
    }
}