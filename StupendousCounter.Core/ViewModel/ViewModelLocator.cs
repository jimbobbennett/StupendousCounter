using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace StupendousCounter.Core.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDatabaseHelper>(() => new DatabaseHelper());
            SimpleIoc.Default.Register<CountersViewModel>();
        }

        public static CountersViewModel Counters => ServiceLocator.Current.GetInstance<CountersViewModel>();
    }
}