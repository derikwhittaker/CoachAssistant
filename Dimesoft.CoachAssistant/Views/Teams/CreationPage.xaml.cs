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

            DataContext = new CreationViewModel(new NavigationService());
        }
    }
}