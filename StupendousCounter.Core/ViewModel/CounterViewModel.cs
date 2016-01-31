using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace StupendousCounter.Core.ViewModel
{
    public class CounterViewModel : ViewModelBase
    {
        private readonly Counter _counter;
        private readonly IDatabaseHelper _databaseHelper;

        public CounterViewModel(Counter counter, IDatabaseHelper databaseHelper)
        {
            _counter = counter;
            _databaseHelper = databaseHelper;
        }

        public string Name => _counter.Name;
        public string Description => _counter.Description;
        public string Value => _counter.Value.ToString("N0");

        private RelayCommand _incrementCommand;
        public RelayCommand IncrementCommand => _incrementCommand ?? (_incrementCommand = new RelayCommand(async () => await Increment()));

        private async Task Increment()
        {
            await _databaseHelper.IncrementCounterAsync(_counter);
            RaisePropertyChanged(() => Value);
        }
    }
}
