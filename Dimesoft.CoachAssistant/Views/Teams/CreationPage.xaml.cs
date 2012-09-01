using System;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Teams;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Dimesoft.CoachAssistant.Views.Teams
{
    public partial class CreationPage : PageBase
    {
        private bool IsSaveEnabled = true;
        public CreationPage()
        {
            InitializeComponent();

            DataContext = new CreationViewModel(new NavigationService(), new EventRepository());
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int teamId = 0;

            if (NavigationContext.QueryString.ContainsKey(QueryStringConstants.TeamId))
            {
                teamId = int.Parse(NavigationContext.QueryString[QueryStringConstants.TeamId]);    
            }

            ViewModel.LoadData(teamId);
        }

        private void AppBarButtonClicked(object sender, EventArgs e)
        {
            if ( IsSaveEnabled )
            {
                if (ViewModel.SaveTeamCommand.CanExecute(null))
                {
                    ViewModel.SaveTeamCommand.Execute(null);
                }
            }
            else
            {
                if (ViewModel.AddPlayerCommand.CanExecute(null))
                {
                    ViewModel.AddPlayerCommand.Execute(null);
                }
            }

        }

        public CreationViewModel ViewModel
        {
            get { return DataContext as CreationViewModel; }
        }

        private void Panorama_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var panelItem = e.AddedItems[0];
            var panel = panelItem as PanoramaItem;

            var appBar = (ApplicationBarIconButton) this.ApplicationBar.Buttons[0];

            if (panel.Name == "TeamInfoPanel")
            {
                IsSaveEnabled = true;
                appBar.Text = "Save Team";
                appBar.IconUri = new Uri("/Images/AppBarIcons/appbar.save.rest.png", UriKind.RelativeOrAbsolute);
            }
            else if (panel.Name == "PlayerPanel")
            {
                IsSaveEnabled = false;
                appBar.Text = "Add Player";
                appBar.IconUri = new Uri("/Images/AppBarIcons/appbar.add.rest.png", UriKind.RelativeOrAbsolute);
            }
        }

    }
}