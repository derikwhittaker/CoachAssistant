using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Dimesoft.CoachAssistant.ViewModels.Teams
{
    public class ListingViewModel : BaseVM
    {
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


            Scheduler.NewThread.Schedule(() =>
            {
                Debug.WriteLine(string.Format("Load Team Data - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

                var teams = _eventRepository.Teams().Select(x =>
                                                                 {
                                                                     var team = new Team(x);
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
                            Teams = new ObservableCollection<Team>(teams);


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

        private ObservableCollection<Team> _teamss;
        public ObservableCollection<Team> Teams
        {
            get { return _teamss; }
            set { _teamss = value; RaisePropertyChanged(() => Teams); }
        }
    }
}