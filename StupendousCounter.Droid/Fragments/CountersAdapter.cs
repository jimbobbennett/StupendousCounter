using System.Collections.Specialized;
using Android.Support.V7.Widget;
using Android.Views;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Droid.Fragments
{
    public class CountersAdapter : RecyclerView.Adapter
    {
        public CountersAdapter()
        {
            ((INotifyCollectionChanged)ViewModelLocator.Counters.Counters).CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            NotifyDataSetChanged();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = ViewModelLocator.Counters.Counters[position];
            ((CounterViewHolder) holder).BindCounterViewModel(item);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.counter_view, parent, false);
            return new CounterViewHolder(itemView);
        }

        public override int ItemCount => ViewModelLocator.Counters.Counters.Count;
    }
}