using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GalaSoft.MvvmLight.Views;
using Moq;
using NUnit.Framework;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Core.Tests.ViewModel
{
    [TestFixture]
    public class CountersViewModelTests
    {
        private Mock<IDatabaseHelper> _mockDatabaseHelper;
        private Mock<INavigationService> _mockNavigationService;

        private readonly Counter _monkeyCounter = new Counter
        {
            Name = "Monkey Count",
            Description = "The number of monkeys",
            Value = 10
        };

        private readonly Counter _platypusCounter = new Counter
        {
            Name = "Playtpus Count",
            Description = "The number of duck-billed platypuses",
            Value = 4
        };

        private static bool Matches(CounterViewModel cvm, Counter counter)
        {
            return cvm.Name == counter.Name &&
                   cvm.Description == counter.Description &&
                   cvm.Value == counter.Value.ToString("N0");
        }

        [SetUp]
        public void SetUp()
        {
            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
            _mockNavigationService = new Mock<INavigationService>();
        }

        [Test]
        public async Task LoadCountersAsyncShouldLoadTheCountersFromTheDatabase()
        {
            _mockDatabaseHelper.Setup(d => d.GetAllCountersAsync()).ReturnsAsync(new List<Counter> { _monkeyCounter, _platypusCounter });
            var vm = new CountersViewModel(_mockDatabaseHelper.Object, _mockNavigationService.Object);
            await vm.LoadCountersAsync();
            vm.Counters.Should().HaveCount(2);
            vm.Counters.Should().Contain(c => Matches(c, _monkeyCounter));
            vm.Counters.Should().Contain(c => Matches(c, _platypusCounter));
        }

        [Test]
        public async Task LoadCountersAsyncShouldClearBeforeLoadingTheCountersFromTheDatabase()
        {
            _mockDatabaseHelper.Setup(d => d.GetAllCountersAsync()).ReturnsAsync(new List<Counter> { _monkeyCounter, _platypusCounter });
            var vm = new CountersViewModel(_mockDatabaseHelper.Object, _mockNavigationService.Object);

            await vm.LoadCountersAsync();
            vm.Counters.Should().HaveCount(2);
            vm.Counters.Should().Contain(c => Matches(c, _monkeyCounter));
            vm.Counters.Should().Contain(c => Matches(c, _platypusCounter));

            await vm.LoadCountersAsync();
            vm.Counters.Should().HaveCount(2);
            vm.Counters.Should().Contain(c => Matches(c, _monkeyCounter));
            vm.Counters.Should().Contain(c => Matches(c, _platypusCounter));
        }

        [Test]
        public void ExecutingAddNewCounterCommandShouldNavigateToTheNewCounterActivity()
        {
            var vm = new CountersViewModel(_mockDatabaseHelper.Object, _mockNavigationService.Object);
            vm.AddNewCounterCommand.Execute(null);
            _mockNavigationService.Verify(n => n.NavigateTo(ViewModelLocator.NewCounterPageKey), Times.Once);
        }

        [Test]
        public void CountersChangingInTheDatabaseShouldReloadTheCounters()
        {
            var vm = new CountersViewModel(_mockDatabaseHelper.Object, _mockNavigationService.Object);
            _mockDatabaseHelper.Raise(d => d.CountersChanged += null, new EventArgs());
            _mockDatabaseHelper.Verify(d => d.GetAllCountersAsync(), Times.Once);
            GC.KeepAlive(vm);
        }
    }
}
