using System;
using System.Windows;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.Views;

namespace Dimesoft.CoachAssistant
{
    public partial class MainPage : PageBase
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            var vm = new MainViewModel( new NavigationService() );
 
            DataContext = vm;

        }


        public MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
            ((MainViewModel)DataContext).LoadData();
        }

        private void ShowAllEventsClicked(object sender, EventArgs e)
        {
            ViewModel.DashboardViewState = DashboardViewState.ShowAll;
        }
        
        private void ShowOnlyActiveClicked(object sender, EventArgs e)
        {
            ViewModel.DashboardViewState = DashboardViewState.ShowActive;
        }
    }
}