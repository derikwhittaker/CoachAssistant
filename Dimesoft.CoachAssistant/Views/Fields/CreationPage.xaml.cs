using System;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Fields;

namespace Dimesoft.CoachAssistant.Views.Fields
{
    public partial class CreationPage : PageBase
    {
        public CreationPage()
        {
            InitializeComponent();

            DataContext = new CreationViewModel(new NavigationService(), new EventRepository());
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int fieldId = 0;

            if (NavigationContext.QueryString.ContainsKey(QueryStringConstants.FieldId))
            {
                fieldId = int.Parse(NavigationContext.QueryString[QueryStringConstants.FieldId]);    
            }

            ViewModel.LoadData(fieldId);
        }

        private void SaveFieldClicked(object sender, EventArgs e)
        {
            if (ViewModel.SaveFieldCommand.CanExecute(null))
            {
                ViewModel.SaveFieldCommand.Execute(null);   
            }
        }

        public CreationViewModel ViewModel
        {
            get { return DataContext as CreationViewModel; }
        }
    }
}