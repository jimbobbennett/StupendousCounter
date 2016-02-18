using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace StupendousCounter.Core.ViewModel
{
    public class NewCounterViewModel : ViewModelBase
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public NewCounterViewModel(IDatabaseHelper databaseHelper, IDialogService dialogService, INavigationService navigationService)
        {
            _databaseHelper = databaseHelper;
            _dialogService = dialogService;
            _navigationService = navigationService;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }

        private RelayCommand _addCounterCommand;
        public RelayCommand AddCounterCommand => _addCounterCommand ?? (_addCounterCommand = new RelayCommand(async () => await AddCounter()));

        private async Task AddCounter()
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

            await _databaseHelper.AddOrUpdateCounterAsync(new Counter {Name = Name, Description = Description});
            _navigationService.GoBack();
        }

        private RelayCommand _goBackCommand;
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(() => _navigationService.GoBack()));
    }
}
