using FluentAssertions;
using GalaSoft.MvvmLight.Views;
using Moq;
using NUnit.Framework;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Core.Tests.ViewModel
{
    [TestFixture]
    public class CounterViewModelTests
    {
        private Mock<IDatabaseHelper> _mockDatabaseHelper;
        private Mock<IDialogService> _mockDialogService;
        private Mock<INavigationService> _mockNavigationService;

        private CounterViewModel CreateCounterViewModel(int value = 10, string description = "Bar", string name = "Foo")
        {
            var counter = new Counter { Name = name, Description = description, Value = value };
            return new CounterViewModel(counter, _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
            _mockDialogService = new Mock<IDialogService>();
            _mockNavigationService = new Mock<INavigationService>();
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
            var vm = new CounterViewModel(counter, _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
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

        [Test]
        public void SettingTheNameRaisesAPropertyChangeEvent()
        {
            var vm = CreateCounterViewModel();
            vm.MonitorEvents();
            vm.Name = "New Name";
            vm.ShouldRaisePropertyChangeFor(v => vm.Name);
        }

        [Test]
        public void SettingTheDescriptionRaisesAPropertyChangeEvent()
        {
            var vm = CreateCounterViewModel();
            vm.MonitorEvents();
            vm.Description = "New Description";
            vm.ShouldRaisePropertyChangeFor(v => vm.Description);
        }

        [Test]
        public void ExecutingTheSaveCommandUpdatesTheChangesOnTheCounter()
        {
            var counter = new Counter {Value = 10, Name = "Name", Description = "Description"};
            var vm = new CounterViewModel(counter, _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Name = "New Name";
            vm.Description = "New Description";
            vm.SaveCommand.Execute(null);
            counter.Name.Should().Be("New Name");
            counter.Description.Should().Be("New Description");
        }

        [Test]
        public void ExecutingTheSaveCommandUpdatesTheCounterInTheDatabase()
        {
            var counter = new Counter { Value = 10, Name = "Name", Description = "Description" };
            var vm = new CounterViewModel(counter, _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Name = "New Name";
            vm.Description = "New Description";
            vm.SaveCommand.Execute(null);
            _mockDatabaseHelper.Verify(d => d.AddOrUpdateCounterAsync(counter));
        }

        [Test]
        public void ExecutingTheSaveCommandNavigatesBack()
        {
            var vm = CreateCounterViewModel();
            vm.SaveCommand.Execute(null);
            _mockNavigationService.Verify(n => n.GoBack());
        }

        [Test]
        public void ExecutingTheDeleteCommandConfirmsTheDelete()
        {
            var vm = CreateCounterViewModel();
            vm.DeleteCommand.Execute(null);
            _mockDialogService.Verify(d => d.ShowMessage($"Are you sure you want to delete {vm.Name}?", "Delete counter", "Yes", "No", null));
        }

        [Test]
        public void ExecutingTheDeleteCommandAndSelectingYesOnTheDialogDeletesTheCounter()
        {
            var counter = new Counter { Value = 10, Name = "Name", Description = "Description" };
            var vm = new CounterViewModel(counter, _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            _mockDialogService.Setup(d => d.ShowMessage($"Are you sure you want to delete {vm.Name}?", "Delete counter", "Yes", "No", null))
                .ReturnsAsync(true);
            vm.DeleteCommand.Execute(null);
            _mockDatabaseHelper.Verify(d => d.DeleteCounterAsync(counter), Times.Once);
        }

        [Test]
        public void ExecutingTheDeleteCommandAndSelectingNoOnTheDialogDoesNotDeleteTheCounter()
        {
            var counter = new Counter { Value = 10, Name = "Name", Description = "Description" };
            var vm = new CounterViewModel(counter, _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            _mockDialogService.Setup(d => d.ShowMessage($"Are you sure you want to delete {vm.Name}?", "Delete counter", "Yes", "No", null))
                .ReturnsAsync(false);
            vm.DeleteCommand.Execute(null);
            _mockDatabaseHelper.Verify(d => d.DeleteCounterAsync(counter), Times.Never);
        }

        [Test]
        public void GoBackCommandNavigatesBackwards()
        {
            var vm = new CounterViewModel(new Counter(), _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.GoBackCommand.Execute(null);
            _mockNavigationService.Verify(n => n.GoBack(), Times.Once);
        }

        [Test]
        public void SaveCommandRaisesAnErrorIfTheNameIsNotSet()
        {
            var vm = new CounterViewModel(new Counter(), _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Description = "Bar";
            vm.SaveCommand.Execute(null);
            _mockDialogService.Verify(d => d.ShowError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            _mockDatabaseHelper.Verify(d => d.AddOrUpdateCounterAsync(It.IsAny<Counter>()), Times.Never);
        }

        [Test]
        public void SaveCommandRaisesAnErrorIfTheDescriptionIsNotSet()
        {
            var vm = new CounterViewModel(new Counter(), _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Name = "Foo";
            vm.SaveCommand.Execute(null);
            _mockDialogService.Verify(d => d.ShowError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            _mockDatabaseHelper.Verify(d => d.AddOrUpdateCounterAsync(It.IsAny<Counter>()), Times.Never);
        }

        [Test]
        public void SaveComandAddsTheCounterAndNavigatesBack()
        {
            var vm = new CounterViewModel(new Counter(), _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Name = "Foo";
            vm.Description = "Bar";
            vm.SaveCommand.Execute(null);
            _mockDialogService.Verify(d => d.ShowError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Never);
            _mockDatabaseHelper.Verify(d => d.AddOrUpdateCounterAsync(It.IsAny<Counter>()), Times.Once);
            _mockNavigationService.Verify(n => n.GoBack(), Times.Once);
        }

        [Test]
        public void ExecutingTheEditCommandNavigatesToTheEditScreen()
        {
            var counter = new Counter();
            var vm = new CounterViewModel(counter, _mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.EditCommand.Execute(null);

            _mockNavigationService.Verify(n => n.NavigateTo(ViewModelLocator.EditCounterPageKey, counter));
        }
    }
}
