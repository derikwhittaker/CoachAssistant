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
    public class DrillListingPanoramaViewModel : BaseVM
    {
        private readonly IDrillsRepository _drillsRepository;
        private readonly ISessonStateService _sessonStateService;
        private IList<Drill> _drills;
        private RelayCommand _selectItemCommand;
        private IList<SportDrills> _sportDrills;

        public DrillListingPanoramaViewModel(IDrillsRepository drillsRepository, ISessonStateService sessonStateService)
        {
            _drillsRepository = drillsRepository;
            _sessonStateService = sessonStateService;
        }

        public void LoadData(int sportId, IList<int> selectedDrills )
        {
            if (DataLoaded) { return; }
            IsBusy = true;
            
            // testing
            SportDrills = new List<SportDrills>
                              {
                                  new SportDrills {SportId = (int)SportType.Soccer, SportName = "Soccer"},
                                  new SportDrills {SportId = (int)SportType.Baseball, SportName = "Baseball"},
                                  new SportDrills {SportId = (int)SportType.Basketball, SportName = "Basketball"}
                              };

            Scheduler.NewThread.Schedule(() =>
            {
                Debug.WriteLine(string.Format("Load data - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

                var drills = _drillsRepository.All().Select( x => new Drill(x)).ToList();

                HandleLoadedCallback(drills);
            });
        }

        private void HandleLoadedCallback(IList<Drill> drills)
        {
            Debug.WriteLine(string.Format("HandleLoadDataCallback - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        Drills = drills;

                                        var drillsBySport = drills.GroupBy(x => x.SportTypeId);

                                        foreach (var drill in drills)
                                        {
                                            var sportDrill = SportDrills.FirstOrDefault(x => x.SportId == drill.SportTypeId);

                                            List<Drill> list = drillsBySport.Where(x => x.Key == drill.SportTypeId).SelectMany(x => x).ToList();

                                            sportDrill.Drills = list;
                                        }
                                        
                                        
                                        IsBusy = false;
                                    });
        }

        private IList<Drill> Drills
        {
            get { return _drills; }
            set { _drills = value; RaisePropertyChanged(() => Drills); }
        }

        public IList<SportDrills> SportDrills
        {
            get { return _sportDrills; }
            set
            {
                _sportDrills = value;
                RaisePropertyChanged(() => SportDrills);
            }
        }
    }

    public class SportDrills : BaseVM
    {
        private string _sportName;
        private int _sportId;
        private IList<Drill> _drills = new List<Drill>();

        public string SportName
        {
            get { return _sportName; }
            set { _sportName = value; RaisePropertyChanged(() => SportName); }
        }

        public int SportId
        {
            get { return _sportId; }
            set { _sportId = value; RaisePropertyChanged(() => SportId); }
        }

        public IList<Drill> Drills
        {
            get { return _drills; }
            set 
            { 
                _drills = value; 
                RaisePropertyChanged(() => Drills); 
                RaisePropertyChanged(() => DrillCount);
            }
        }

        public string DrillCount
        {
            get { return string.Format("{0} Drills", _drills.Count); }
        }
    }
}
