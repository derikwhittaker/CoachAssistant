using System;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels;
using Dimesoft.CoachAssistant.ViewModels.Teams;
using Microsoft.Phone.Controls;

namespace Dimesoft.CoachAssistant.Views
{
    public partial class TeamListingPage : PageBase
    {
        public TeamListingPage()
        {
            InitializeComponent();
            DataContext = new ListingViewModel(new NavigationService(), new EventRepository());
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs args)
        {
            ViewModel.LoadData();
        }

        private void CreateNewTeamClicked(object sender, EventArgs e)
        {
            ((ListingViewModel)DataContext).CreateNewTeamCommand.Execute(null);
        }

        public ListingViewModel ViewModel
        {
            get { return DataContext as ListingViewModel; }
        }
    }
}