using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace StupendousCounter.Core.ViewModel
{
    public static class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDatabaseHelper>(() => new DatabaseHelper());
            SimpleIoc.Default.Register<CountersViewModel>();
        }

        public static void RegisterNavigationService(INavigationService navigationService)
        {
            SimpleIoc.Default.Register(() => navigationService);
        }

        public static void RegisterDialogService(IDialogService dialogService)
        {
            SimpleIoc.Default.Register(() => dialogService);
        }

        public const string NewCounterPageKey = "NewCounterPage";
        public const string EditCounterPageKey = "EditCounterPage";

        public static CountersViewModel Counters => ServiceLocator.Current.GetInstance<CountersViewModel>();
        public static INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();
        public static IDatabaseHelper DatabaseHelper => ServiceLocator.Current.GetInstance<IDatabaseHelper>();
        public static IDialogService DialogService => ServiceLocator.Current.GetInstance<IDialogService>();
    }
}