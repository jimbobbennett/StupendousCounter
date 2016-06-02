using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace StupendousCounter.Core.ViewModel
{
    public class CountersViewModel : ViewModelBase
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private readonly ObservableCollection<CounterViewModel> _counters = new ObservableCollection<CounterViewModel>();
        public ReadOnlyObservableCollection<CounterViewModel> Counters { get; private set; }

        public CountersViewModel(IDatabaseHelper databaseHelper, INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _databaseHelper = databaseHelper;
            _databaseHelper.CountersChanged += async (s, e) => await LoadCountersAsync();
            Counters = new ReadOnlyObservableCollection<CounterViewModel>(_counters);
        }

        public async Task LoadCountersAsync()
        {
            var counters = await _databaseHelper.GetAllCountersAsync();
            _counters.Clear();
            foreach (var counter in counters)
                _counters.Add(new CounterViewModel(counter, _databaseHelper, _dialogService, _navigationService));
        }

        private RelayCommand _addNewCounterCommand;
        public RelayCommand AddNewCounterCommand => _addNewCounterCommand ?? (_addNewCounterCommand = new RelayCommand(AddNewCounter));

        private void AddNewCounter()
        {
            _navigationService.NavigateTo(ViewModelLocator.NewCounterPageKey);
        }
    }
}
