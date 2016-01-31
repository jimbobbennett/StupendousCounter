using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Core.Tests.ViewModel
{
    [TestFixture]
    public class CountersViewModelTests
    {
        private Mock<IDatabaseHelper> _mockDatabaseHelper;

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
        }

        [Test]
        public async Task LoadCountersAsyncShouldLoadTheCountersFromTheDatabase()
        {
            _mockDatabaseHelper.Setup(d => d.GetAllCountersAsync()).ReturnsAsync(new List<Counter> {_monkeyCounter, _platypusCounter});
            var vm = new CountersViewModel(_mockDatabaseHelper.Object);
            await vm.LoadCountersAsync();
            vm.Counters.Should().HaveCount(2);
            vm.Counters.Should().Contain(c => Matches(c, _monkeyCounter));
            vm.Counters.Should().Contain(c => Matches(c, _platypusCounter));
        }
    }
}
