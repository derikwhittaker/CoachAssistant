using System;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Teams;
using Microsoft.Phone.Controls;

namespace Dimesoft.CoachAssistant.Views.Teams
{
    public partial class CreationPage : PhoneApplicationPage
    {
        public CreationPage()
        {
            InitializeComponent();

            DataContext = new CreationViewModel(new NavigationService(), new EventRepository());
        }

        private void SaveTeamClicked(object sender, EventArgs e)
        {
            if (ViewModel.SaveTeamCommand.CanExecute(null))
            {
                ViewModel.SaveTeamCommand.Execute(null);   
            }
        }

        public CreationViewModel ViewModel
        {
            get { return DataContext as CreationViewModel; }
        }
    }
}