using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace StupendousCounter.Droid.Fragments
{
    public class CountersFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static CountersFragment NewInstance()
        {
            return new CountersFragment { Arguments = new Bundle() };
        }

        private RecyclerView _recyclerView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.counters_fragment, null);
            
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.countersRecyclerView);
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false));
            _recyclerView.SetAdapter(new CountersAdapter());

            return view;
        }
    }
}