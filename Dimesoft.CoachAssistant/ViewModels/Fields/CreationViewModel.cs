using System;
using System.Collections.Generic;
using System.Linq;
using Dimesoft.CoachAssistant.Domain.Models;
using Dimesoft.CoachAssistant.Domain.Repositories;
using Dimesoft.CoachAssistant.Models;
using Dimesoft.CoachAssistant.Services;
using GalaSoft.MvvmLight.Command;

namespace Dimesoft.CoachAssistant.ViewModels.Fields
{
    public class CreationViewModel : BaseVM
    {
        private readonly INavigationService _navigationService;
        private readonly IEventRepository _eventRepository;
        private RelayCommand _saveFieldCommand;
        private LocationDto _currentField = new LocationDto();
        
        public CreationViewModel(INavigationService navigationService, IEventRepository eventRepository)
        {
            _navigationService = navigationService;
            _eventRepository = eventRepository;

            PageTitle = "Field Maintenance";
            }

        public void LoadData( int locationId )
        {
            _currentField = _eventRepository.Locations().FirstOrDefault(x => x.Id == locationId) ?? new LocationDto();

            RaisePropertyChanged(() => FieldName);
        }

        public RelayCommand SaveFieldCommand
        {
            get { return _saveFieldCommand ?? (_saveFieldCommand = new RelayCommand(SaveField, CanSaveField)); }
        }

        private bool CanSaveField()
        {
            return !string.IsNullOrEmpty(FieldName);
        }

        private void SaveField()
        {

            _eventRepository.Save(_currentField);

            _navigationService.GoBack();
        }
        
        public string FieldName
        {
            get { return _currentField.Name; }
            set
            {
                _currentField.Name = value; 
                RaisePropertyChanged(() => FieldName);
                SaveFieldCommand.RaiseCanExecuteChanged();
            }
        }

    }
}