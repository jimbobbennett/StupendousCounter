using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace StupendousCounter.Droid.Fragments
{
    public class AboutFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static AboutFragment NewInstance()
        {
            var frag2 = new AboutFragment { Arguments = new Bundle() };
            return frag2;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.about_fragment, null);
        }
    }
}