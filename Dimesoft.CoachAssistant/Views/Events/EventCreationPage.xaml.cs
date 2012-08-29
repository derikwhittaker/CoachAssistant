using System;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.ViewModels.Events;

namespace Dimesoft.CoachAssistant.Views.Events
{
    public partial class EventCreationPage : PageBase
    {
        public EventCreationPage()
        {
            InitializeComponent();

            var vm = new EventCreationViewModel( new EventRepository() );

            vm.PropertyChanged += (s, e) =>
                                      {
                                          if (e.PropertyName == "SelectedTeam")
                                          {
                                              var sportId = ViewModel.SelectedTeam.Dto.SportTypeId;
                                              BackgroundImage.ImageSource = ViewModel.GetBackgroundImage(sportId);
                                          }
                                      };

            DataContext = vm;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.LoadData();

            BackgroundImage.ImageSource = ViewModel.GetBackgroundImage(0);
        }

        public EventCreationViewModel ViewModel
        {
            get { return DataContext as EventCreationViewModel; }
        }

        private void SaveEventClicked(object sender, EventArgs e)
        {
            ViewModel.SaveEventCommand.Execute(null);
        }
    }
}