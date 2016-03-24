using Android.App;
using Android.OS;
using Android.Views;
using JimBobBennett.MvvmLight.AppCompat;
using StupendousCounter.Core;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Droid.Activities
{
    [Activity(Label = "Edit Counter")]
    public class EditCounterActivity : NewCounterActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Title = $"Edit {ViewModel.Name}";

            Name.SetSelection(Name.Text.Length);
        }

        protected override CounterViewModel GetCounterViewModel()
        {
            var navigationService = ViewModelLocator.NavigationService;
            var counter = (Counter) ((AppCompatNavigationService) navigationService).GetAndRemoveParameter(Intent);
            return new CounterViewModel(counter,
                ViewModelLocator.DatabaseHelper,
                ViewModelLocator.DialogService,
                navigationService);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            Toolbar.InflateMenu(Resource.Menu.edit_counter_menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_delete_counter:
                    ViewModel.DeleteCommand.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}