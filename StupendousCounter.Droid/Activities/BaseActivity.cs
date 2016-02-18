using Android.OS;
using Android.Support.V7.Widget;
using JimBobBennett.MvvmLight.AppCompat;

namespace StupendousCounter.Droid.Activities
{
    public abstract class BaseActivity : AppCompatActivityBase
    {
        public Toolbar Toolbar { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(LayoutResource);
            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }
        }

        protected abstract int LayoutResource { get; }

        protected int ActionBarIcon
        {
            set { Toolbar.SetNavigationIcon(value); }
        }
    }
}

