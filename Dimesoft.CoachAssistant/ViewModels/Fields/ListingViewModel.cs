using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Dimesoft.CoachAssistant.Common;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;

namespace Dimesoft.CoachAssistant.ViewModels.Fields
{
    public class ListingViewModel : BaseVM
    {

        private ObservableCollection<Field> _fields;
        private Field _selectedField;
        private readonly INavigationService _navigationService;
        private readonly IEventRepository _eventRepository;

        public ListingViewModel( INavigationService navigationService, IEventRepository eventRepository)
        {
            _navigationService = navigationService;
            _eventRepository = eventRepository;

            PageTitle = "Fields Listing";
        }

        public void LoadData()
        {
            if (DataLoaded) { return; }
            IsBusy = true;


            Scheduler.NewThread.Schedule(() =>
            {
                Debug.WriteLine(string.Format("Load Field Data - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

                var fields = _eventRepository.Locations().Select(x =>
                                                                 {
                                                                     var field = new Field(x);
                                                                     return field;

                                                                 }).OrderBy(x => x.Name).ToList();

                HandleLoadedCallback(fields);
            });
        }

        private void HandleLoadedCallback(IEnumerable<Field> fields)
        {
            Debug.WriteLine(string.Format("HandleLoadDataCallback - {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Fields = new ObservableCollection<Field>(fields);

                            IsBusy = false;
                        });
        }

        private RelayCommand _createNewTeamCommand;
        public RelayCommand CreateNewFieldCommand
        {
            get { return _createNewTeamCommand ?? (_createNewTeamCommand = new RelayCommand(CreateNewField)); }
        }

        private void CreateNewField()
        {
            var url = string.Format("/Views/Fields/CreationPage.xaml");

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        public ObservableCollection<Field> Fields
        {
            get { return _fields; }
            set { _fields = value; RaisePropertyChanged(() => Fields); }
        }

        public Field SelectedField
        {
            get { return _selectedField; }
            set
            {
                _selectedField = value;

                EditSelectedTeam(value);
            }
        }

        private void EditSelectedTeam(Field field)
        {
            if (field == null) { return; }

            var url = string.Format("/Views/Fields/CreationPage.xaml?{0}={1}", QueryStringConstants.FieldId, field.Id);

            _navigationService.NavigateTo(new Uri(url, UriKind.RelativeOrAbsolute));
        }
    }
}