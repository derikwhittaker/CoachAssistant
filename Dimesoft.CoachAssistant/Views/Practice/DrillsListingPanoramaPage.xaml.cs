using System.Collections.Generic;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Services;
using Dimesoft.CoachAssistant.ViewModels.Practice;
using Microsoft.Phone.Controls;

namespace Dimesoft.CoachAssistant.Views.Practice
{
    public partial class DrillsListingPanoramaPage : PhoneApplicationPage
    {
        public DrillsListingPanoramaPage()
        {
            InitializeComponent();

            var vm = new DrillListingPanoramaViewModel(new DrillsRepository(), new SessonStateService());

            DataContext = vm;

            // testing
            vm.LoadData(0, new List<int>());
        }
    }
}