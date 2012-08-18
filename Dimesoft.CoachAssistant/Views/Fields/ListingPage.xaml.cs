using System;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Fields;
using Microsoft.Phone.Controls;

namespace Dimesoft.CoachAssistant.Views.Fields
{
    public partial class ListingPage : PageBase
    {
        public ListingPage()
        {
            InitializeComponent();
            DataContext = new ListingViewModel(new NavigationService(), new EventRepository());
        }

        protected override void OnLoaded(object sender, System.Windows.RoutedEventArgs args)
        {
            ViewModel.LoadData();
        }

        private void CreateNewFieldClicked(object sender, EventArgs e)
        {
            ViewModel.CreateNewFieldCommand.Execute(null);
        }

        public ListingViewModel ViewModel
        {
            get { return DataContext as ListingViewModel; }
        }

    }
}