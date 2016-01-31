using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace StupendousCounter.Core.ViewModel
{
    public class CountersViewModel : ViewModelBase
    {
        private readonly IDatabaseHelper _databaseHelper;

        private readonly ObservableCollection<CounterViewModel> _counters = new ObservableCollection<CounterViewModel>();
        public ReadOnlyObservableCollection<CounterViewModel> Counters { get; private set; }

        public CountersViewModel(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            Counters = new ReadOnlyObservableCollection<CounterViewModel>(_counters);
        }

        public async Task LoadCountersAsync()
        {
            var counters = await _databaseHelper.GetAllCountersAsync();
            foreach (var counter in counters)
            {
                _counters.Add(new CounterViewModel(counter, _databaseHelper));
            }
        }
    }
}
