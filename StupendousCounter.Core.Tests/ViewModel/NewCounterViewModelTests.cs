using FluentAssertions;
using GalaSoft.MvvmLight.Views;
using Moq;
using NUnit.Framework;
using StupendousCounter.Core.ViewModel;

namespace StupendousCounter.Core.Tests.ViewModel
{
    [TestFixture]
    public class NewCounterViewModelTests
    {
        private Mock<IDatabaseHelper> _mockDatabaseHelper;
        private Mock<IDialogService> _mockDialogService;
        private Mock<INavigationService> _mockNavigationService;

        [SetUp]
        public void SetUp()
        {
            _mockDatabaseHelper = new Mock<IDatabaseHelper>();
            _mockDialogService = new Mock<IDialogService>();
            _mockNavigationService = new Mock<INavigationService>();
        }

        [Test]
        public void SettingTheNameRaisesAPropertyChangedEvent()
        {
            var vm = new NewCounterViewModel(_mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.MonitorEvents();
            vm.Name = "Foo";
            vm.ShouldRaisePropertyChangeFor(v => v.Name);
        }

        [Test]
        public void SettingTheDescriptionRaisesAPropertyChangedEvent()
        {
            var vm = new NewCounterViewModel(_mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.MonitorEvents();
            vm.Description = "Foo";
            vm.ShouldRaisePropertyChangeFor(v => v.Description);
        }

        [Test]
        public void GoBackCommandNavigatesBackwards()
        {
            var vm = new NewCounterViewModel(_mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.GoBackCommand.Execute(null);
            _mockNavigationService.Verify(n => n.GoBack(), Times.Once);
        }

        [Test]
        public void AddCommandRaisesAnErrorIfTheNameIsNotSet()
        {
            var vm = new NewCounterViewModel(_mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Description = "Bar";
            vm.AddCounterCommand.Execute(null);
            _mockDialogService.Verify(d => d.ShowError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            _mockDatabaseHelper.Verify(d => d.AddOrUpdateCounterAsync(It.IsAny<Counter>()), Times.Never);
        }

        [Test]
        public void AddCommandRaisesAnErrorIfTheDescriptionIsNotSet()
        {
            var vm = new NewCounterViewModel(_mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Name = "Foo";
            vm.AddCounterCommand.Execute(null);
            _mockDialogService.Verify(d => d.ShowError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            _mockDatabaseHelper.Verify(d => d.AddOrUpdateCounterAsync(It.IsAny<Counter>()), Times.Never);
        }

        [Test]
        public void AddComandAddsTheCounterAndNavigatesBack()
        {
            var vm = new NewCounterViewModel(_mockDatabaseHelper.Object, _mockDialogService.Object, _mockNavigationService.Object);
            vm.Name = "Foo";
            vm.Description = "Bar";
            vm.AddCounterCommand.Execute(null);
            _mockDialogService.Verify(d => d.ShowError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Never);
            _mockDatabaseHelper.Verify(d => d.AddOrUpdateCounterAsync(It.IsAny<Counter>()), Times.Once);
            _mockNavigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }
}
