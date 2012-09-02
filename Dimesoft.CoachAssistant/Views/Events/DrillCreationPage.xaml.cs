using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Events;

namespace Dimesoft.CoachAssistant.Views.Events
{
    public partial class DrillCreationPage : PageBase
    {
        public int SportId { get; set; }
        public string CallingPageName { get; set; }

        public DrillCreationPage()
        {
            InitializeComponent();

            var vm = new DrillCreationViewModel(new DrillsRepository(), new EventRepository(), new SessonStateService(), new NavigationService());

            vm.PropertyChanged += (s, e) =>
                                      {
                                          if (e.PropertyName == "CurrentSportId")
                                          {
                                              var sportId = ViewModel.CurrentSportId;
                                              BackgroundImage.ImageSource = ViewModel.GetBackgroundImage(sportId);
                                          }
                                      };

            DataContext = vm;
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