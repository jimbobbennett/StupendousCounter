using FluentAssertions;
using Moq;
using NUnit.Framework;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Core.Tests.ViewModel
{
    [TestFixture]
    public class CounterViewModelTests
    {
        private Mock<IDatabaseHelper> _mockDatabaseHelper;

        private CounterViewModel CreateCounterViewModel(int value = 10, string description = "Bar", string name = "Foo")
        {
            var counter = new Counter { Name = name, Description = description, Value = value };
            return new CounterViewModel(counter, _mockDatabaseHelper.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
        }

        [Test]
        public void NameShouldComeFromTheCounter()
        {
            CreateCounterViewModel().Name.Should().Be("Foo");
        }

        [Test]
        public void DescriptionShouldComeFromTheCounter()
        {
            CreateCounterViewModel().Description.Should().Be("Bar");
        }

        [Test]
        public void ValueShouldComeFromTheCounter()
        {
            CreateCounterViewModel().Value.Should().Be("10");
        }

        [Test]
        public void ExecutingTheIncrementCommandShouldIncrementTheValueInTheDatabaseHelper()
        {
            _mockDatabaseHelper.Setup(d => d.IncrementCounterAsync(It.IsAny<Counter>())).Callback<Counter>(c => ++c.Value);
            var counter = new Counter {Value = 10};
            var vm = new CounterViewModel(counter, _mockDatabaseHelper.Object);
            vm.IncrementCommand.Execute(null);
            _mockDatabaseHelper.Verify(d => d.IncrementCounterAsync(counter));
            vm.Value.Should().Be("11");
        }

        [Test]
        public void ExecutingTheIncrementCommandShouldRaiseAPropertyChangeForValue()
        {
            var vm = CreateCounterViewModel();
            vm.MonitorEvents();
            vm.IncrementCommand.Execute(null);
            vm.ShouldRaisePropertyChangeFor(v => vm.Value);
        }
    }
}
