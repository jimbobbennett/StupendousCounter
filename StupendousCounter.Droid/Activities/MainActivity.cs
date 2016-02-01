using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using StupendousCounter.Droid.Fragments;
using Android.Support.Design.Widget;
using StupendousCounter.Core;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Droid.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon")]
    public class MainActivity : BaseActivity
    {

        DrawerLayout drawerLayout;
        NavigationView navigationView;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.main;
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(path, "counters.db3");
            DatabaseHelper.CreateDatabase(dbPath);

#if DEBUG
            await AddDummyData();
#endif

            await ViewModelLocator.Counters.LoadCountersAsync();

            drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //handle navigation
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_counters:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_about:
                        ListItemClicked(1);
                        break;
                }
                
                drawerLayout.CloseDrawers();
            };


            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }
        }
        
        private static async Task AddDummyData()
        {
            var dbHelper = new DatabaseHelper();
            if (!(await dbHelper.GetAllCountersAsync()).Any())
            {
                var counter1 = new Counter
                {
                    Name = "Monkey Count",
                    Description = "The number of monkeys",
                    Value = 10
                };

                var counter2 = new Counter
                {
                    Name = "Playtpus Count",
                    Description = "The number of duck-billed platypuses",
                    Value = 4
                };

                await dbHelper.AddOrUpdateCounterAsync(counter1);
                await dbHelper.AddOrUpdateCounterAsync(counter2);
            }
        }

        int oldPosition = -1;
        private void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == oldPosition)
                return;

            oldPosition = position;

            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = CountersFragment.NewInstance();
                    break;
                case 1:
                    fragment = AboutFragment.NewInstance();
                    break;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

