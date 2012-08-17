using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Dimesoft.CoachAssistant.ViewModels.Practice
{
    public class DrillListingViewModel : BaseVM
    {
        private readonly IDrillsRepository _drillsRepository;
        private readonly ISessonStateService _sessonStateService;
        private IList<Drill> _drills;
        private RelayCommand _selectItemCommand;

        public DrillListingViewModel( IDrillsRepository drillsRepository, ISessonStateService sessonStateService)
        {
            _drillsRepository = drillsRepository;
            _sessonStateService = sessonStateService;
        }

        public void LoadData(int sportId, IList<int> selectedDrills )
        {
            if (DataLoaded) { return; }
            IsBusy = true;

            CurrentSportType = (SportType)Enum.Parse(typeof(SportType), sportId.ToString(), true);

            Scheduler.NewThread.Schedule(() =>
            {
                Debug.WriteLine(string.Format("Load data - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

                var drills = _drillsRepository.ForSport(CurrentSportType).Select(x =>
                                                                                     {

                                                                                         var drill = new Drill(x);

                                                                                         drill.Selected = selectedDrills.Any(y => y == x.Id);

                                                                                         return drill;
                                                                                     }).OrderBy(x => x.Name).ToList();

                HandleLoadedCallback(drills);
            });
        }

        private void HandleLoadedCallback(IList<Drill> drills)
        {
            Debug.WriteLine(string.Format("HandleLoadDataCallback - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        Drills = drills;

                                        IsBusy = false;
                                    });
        }

        public RelayCommand SelectItemCommand
        {
            get { return _selectItemCommand ?? (_selectItemCommand = new RelayCommand(ItemsSelected)); }
        }

        private void ItemsSelected()
        {
            List<int> selectedDrills = Drills.Where(x => x.Selected).Select(x => x.Id).ToList();

            _sessonStateService.Set(SessoinStateConstants.SelectedDrills, selectedDrills);
        }

        public SportType CurrentSportType { get; set; }

        public IList<Drill> Drills
        {
            get { return _drills; }
            set { _drills = value; RaisePropertyChanged(() => Drills); }
        }
    }
}
