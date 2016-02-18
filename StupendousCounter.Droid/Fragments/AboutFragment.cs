using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace StupendousCounter.Droid.Fragments
{
    public class AboutFragment : Fragment
    {
        public static AboutFragment NewInstance()
        {
            var frag2 = new AboutFragment { Arguments = new Bundle() };
            return frag2;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.about_fragment, null);
        }
    }
}