using System;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Teams;

namespace Dimesoft.CoachAssistant.Views.Teams
{
    public partial class PlayerCreationPage : PageBase
    {
        private int PlayerId;
        public PlayerCreationPage()
        {
            InitializeComponent();

            var vm = new PlayerCreationViewModel(new NavigationService(), new EventRepository());

            DataContext = vm;
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs args)
        {
            ViewModel.LoadData(PlayerId);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            PlayerId = int.Parse(NavigationContext.QueryString[QueryStringConstants.PlayerId]);

        }

        public PlayerCreationViewModel ViewModel
        {
            get { return DataContext as PlayerCreationViewModel; }
        }

        private void SaveEventClicked(object sender, EventArgs e)
        {
            
        }
    }
}