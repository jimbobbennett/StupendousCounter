using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace StupendousCounter.Core.ViewModel
{
    public class CounterViewModel : ViewModelBase
    {
        private readonly Counter _counter;
        private readonly IDatabaseHelper _databaseHelper;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public CounterViewModel(Counter counter, IDatabaseHelper databaseHelper, IDialogService dialogService, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            _counter = counter;
            _databaseHelper = databaseHelper;

            Name = counter.Name;
            Description = counter.Description;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
        
        public string Value => _counter.Value.ToString("N0");

        private RelayCommand _incrementCommand;
        public RelayCommand IncrementCommand => _incrementCommand ?? (_incrementCommand = new RelayCommand(async () => await IncrementAsync()));

        private async Task IncrementAsync()
        {
            await _databaseHelper.IncrementCounterAsync(_counter);
            RaisePropertyChanged(() => Value);
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(async () => await SaveAsync()));

        private async Task SaveAsync()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await _dialogService.ShowError("The name must be set", "No name", "OK", null);
                return;
            }

            if (string.IsNullOrEmpty(Description))
            {
                await _dialogService.ShowError("The description must be set", "No description", "OK", null);
                return;
            }

            _counter.Name = Name;
            _counter.Description = Description;
            await _databaseHelper.AddOrUpdateCounterAsync(_counter);
            _navigationService.GoBack();
        }
        
        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(async () => await DeleteAsync()));

        private async Task DeleteAsync()
        {
            if (await _dialogService.ShowMessage($"Are you sure you want to delete {Name}?", "Delete counter", "Yes", "No", null))
            {
                await _databaseHelper.DeleteCounterAsync(_counter);
                _navigationService.GoBack();
            }
        }

        private RelayCommand _editCommand;
        public RelayCommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand( Edit));

        private void Edit()
        {
            _navigationService.NavigateTo(ViewModelLocator.EditCounterPageKey, _counter);
        }

        private RelayCommand _goBackCommand;
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(() => _navigationService.GoBack()));
    }
}
