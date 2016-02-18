using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Droid.Activities
{
    [Activity(Label = "New Counter")]
    public class NewCounterActivity : BaseActivity
    {
        public NewCounterViewModel ViewModel { get; private set; }

        private readonly List<Binding> _bindings = new List<Binding>();

        private EditText _name;
        public EditText Name => _name ?? (_name = FindViewById<EditText>(Resource.Id.new_counter_name));

        private EditText _description;
        public EditText Description => _description ?? (_description = FindViewById<EditText>(Resource.Id.new_counter_description));

        private Button _createCounter;
        public Button CreateCounter => _createCounter ?? (_createCounter = FindViewById<Button>(Resource.Id.new_counter_create));

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            ViewModel = ViewModelLocator.NewCounter;
            ViewModel.Name = string.Empty;
            ViewModel.Description = string.Empty;

            Bind();
        }

        private void Bind()
        {
            _bindings.Add(this.SetBinding(() => ViewModel.Name, () => Name.Text, BindingMode.TwoWay));
            _bindings.Add(this.SetBinding(() => ViewModel.Description, () => Description.Text, BindingMode.TwoWay));

            CreateCounter.SetCommand(nameof(Button.Click), ViewModel.AddCounterCommand);
        }

        protected override int LayoutResource => Resource.Layout.new_counter;

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                ViewModel.GoBackCommand.Execute(null);
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}