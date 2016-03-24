using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using StupendousCounter.Core;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Droid.Activities
{
    [Activity(Label = "New Counter")]
    public class NewCounterActivity : BaseActivity
    {
        public CounterViewModel ViewModel { get; private set; }

        private readonly List<Binding> _bindings = new List<Binding>();

        private EditText _name;
        public EditText Name => _name ?? (_name = FindViewById<EditText>(Resource.Id.new_counter_name));

        private EditText _description;
        public EditText Description => _description ?? (_description = FindViewById<EditText>(Resource.Id.new_counter_description));
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = GetCounterViewModel();

            Bind();
        }

        protected virtual CounterViewModel GetCounterViewModel()
        {
            return new CounterViewModel(new Counter(),
                ViewModelLocator.DatabaseHelper,
                ViewModelLocator.DialogService,
                ViewModelLocator.NavigationService);
        }

        private void Bind()
        {
            _bindings.Add(this.SetBinding(() => ViewModel.Name, () => Name.Text, BindingMode.TwoWay));
            _bindings.Add(this.SetBinding(() => ViewModel.Description, () => Description.Text, BindingMode.TwoWay));
        }

        protected override int LayoutResource => Resource.Layout.new_counter;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            Toolbar.InflateMenu(Resource.Menu.new_counter_menu);
            
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    ViewModel.GoBackCommand.Execute(null);
                    return true;
                case Resource.Id.action_save_counter:
                    ViewModel.SaveCommand.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}