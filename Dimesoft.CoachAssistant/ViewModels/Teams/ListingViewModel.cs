using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Dimesoft.CoachAssistant.ViewModels.Teams
{
    public class ListingViewModel : BaseVM
    {

        private ObservableCollection<Team> _teamss;
        private Team _selectedTeam;
        private readonly INavigationService _navigationService;
        private readonly IEventRepository _eventRepository;

        public ListingViewModel( INavigationService navigationService, IEventRepository eventRepository)
        {
            _navigationService = navigationService;
            _eventRepository = eventRepository;

            PageTitle = "Teams Listing";
        }

        public void LoadData()
        {
            if (DataLoaded) { return; }
            IsBusy = true;

            var sports = new List<Sport>
                         {
                             new Sport {Id = (int) SportType.Unknown, Name = "No Selection Made"},
                             new Sport {Id = (int) SportType.Soccer, Name = "Soccer"},
                             new Sport {Id = (int) SportType.Baseball, Name = "Baseball"},
                             new Sport {Id = (int) SportType.Basketball, Name = "Basketball"}
                         };

            Scheduler.NewThread.Schedule(() =>
            {
                Debug.WriteLine(string.Format("Load Team Data - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
                
                var teams = _eventRepository.Teams().Select(x =>
                                                                {
                                                                    var sport = sports.FirstOrDefault(s => s.Id == x.SportTypeId);
                                                                     var team = new Team(x, sport);
                                                                     return team;

                                                                 }).OrderBy(x => x.Name).ToList();

                HandleLoadedCallback(teams);
            });
        }

        private void HandleLoadedCallback(IEnumerable<Team> teams)
        {
            Debug.WriteLine(string.Format("HandleLoadDataCallback - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Teams = new ObservableCollection<Team>(teams.OrderByDescending(x => x.MyTeam).ThenBy(y => y.Name));
                            
                            IsBusy = false;
                        });
        }

        private RelayCommand _createNewTeamCommand;        

        public RelayCommand CreateNewTeamCommand
        {
            get { return _createNewTeamCommand ?? (_createNewTeamCommand = new RelayCommand(CreateNewTeam)); }
        }

        private void CreateNewTeam()
        {
            var url = string.Format("/Views/Teams/CreationPage.xaml");

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        public ObservableCollection<Team> Teams
        {
            get { return _teamss; }
            set { _teamss = value; RaisePropertyChanged(() => Teams); }
        }

        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;

                EditSelectedTeam(value);
            }
        }

        private void EditSelectedTeam(Team team)
        {
            if (team == null) { return; }

            var url = string.Format("/Views/Teams/CreationPage.xaml?{0}={1}", QueryStringConstants.TeamId, team.Id);

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }

    }
}