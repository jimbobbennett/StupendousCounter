using System;
using System.ComponentModel;
using Android.App;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Droid.Fragments
{
    public class CounterViewHolder : RecyclerView.ViewHolder
    {
        private readonly TextView _name;
        private readonly TextView _description;
        private readonly TextView _value;

        private CounterViewModel _counterViewModel;

        public CounterViewHolder(View itemView) : base(itemView)
        {
            _name = itemView.FindViewById<TextView>(Resource.Id.counter_name);
            _description = itemView.FindViewById<TextView>(Resource.Id.counter_description);
            _value = itemView.FindViewById<TextView>(Resource.Id.counter_value);

            var increment = itemView.FindViewById<ImageButton>(Resource.Id.counter_increment);
            increment.SetColorFilter(new Color(ContextCompat.GetColor(Application.Context, Resource.Color.primaryDark)));
            increment.Click += IncrementOnClick;

            itemView.LongClick += ItemLongClick;
        }

        private void ItemLongClick(object sender, View.LongClickEventArgs e)
        {
            _counterViewModel.EditCommand.Execute(null);
        }

        private void IncrementOnClick(object sender, EventArgs eventArgs)
        {
            _counterViewModel.IncrementCommand.Execute(null);
        }

        public void BindCounterViewModel(CounterViewModel counterViewModel)
        {
            if (_counterViewModel != null)
                _counterViewModel.PropertyChanged -= CounterViewModelOnPropertyChanged;

            _counterViewModel = counterViewModel;
            _counterViewModel.PropertyChanged += CounterViewModelOnPropertyChanged;

            _name.Text = counterViewModel.Name;
            _description.Text = counterViewModel.Description;
            _value.Text = counterViewModel.Value;
        }

        private void CounterViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(CounterViewModel.Value))
                _value.Text = _counterViewModel.Value;
        }
    }
}