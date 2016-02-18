using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using GalaSoft.MvvmLight.Helpers;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Droid.Fragments
{
    public class CountersFragment : Fragment
    {
        public static CountersFragment NewInstance()
        {
            return new CountersFragment { Arguments = new Bundle() };
        }

        private RecyclerView _recyclerView;
        private FloatingActionButton _floatingActionButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.counters_fragment, null);
            
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.countersRecyclerView);
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false));
            _recyclerView.SetAdapter(new CountersAdapter());

            _floatingActionButton = view.FindViewById<FloatingActionButton>(Resource.Id.floatingAddNewCounterButton);
            _floatingActionButton.SetCommand(nameof(FloatingActionButton.Click), ViewModelLocator.Counters.AddNewCounterCommand);

            return view;
        }
    }
}