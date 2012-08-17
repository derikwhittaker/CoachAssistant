using Dimesoft.CoachAssistant.Services;

namespace Dimesoft.CoachAssistant.ViewModels.Teams
{
    public class CreationViewModel : BaseVM
    {
        private readonly INavigationService _navigationService;

        public CreationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            PageTitle = "Team Maintenance";
        }
    }
}