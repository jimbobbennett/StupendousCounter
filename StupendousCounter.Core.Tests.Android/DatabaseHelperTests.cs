using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Java.Lang;
using NUnit.Framework;

namespace StupendousCounter.Core.Tests.Android
{
    [TestFixture]
    public class DatabaseHelperTests
    {
        private static readonly string RootPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        
        [Test]
        public void InsertingACounterShouldSetItsIdAsync()
        {
            var dbfile = Path.Combine(RootPath, Guid.NewGuid().ToString("N") + ".db3");

            DatabaseHelper.CreateDatabase(dbfile);

            var db = new DatabaseHelper();
            var counter = new Counter
            {
                Name = "TestCounter",
                Description = "A test counter"
            };

            counter.Id.Should().Be(0);

            var res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter);
                return 0;
            }).Result;

            counter.Id.Should().Be(1);
        }

        [Test]
        public void UpdatingTheCountersNameShouldUpdateTheValueInTheDatabase()
        {
            var dbfile = Path.Combine(RootPath, Guid.NewGuid().ToString("N") + ".db3");

            DatabaseHelper.CreateDatabase(dbfile);

            var db = new DatabaseHelper();
            var counter = new Counter
            {
                Name = "TestCounter",
                Description = "A test counter"
            };

            counter.Id.Should().Be(0);

            var res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter);
                return 0;
            }).Result;

            counter.Name = "RenamedCounter";

            res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter);
                return 0;
            }).Result;

            var newCounter = Task.Run(async () => (await db.GetAllCountersAsync()).Single()).Result;

            newCounter.Should().NotBeSameAs(counter);
            newCounter.Id.Should().Be(counter.Id);
            newCounter.Name.Should().Be(counter.Name);
            newCounter.Description.Should().Be(counter.Description);
        }

        [Test]
        public void UpdatingTheCountersDescriptionShouldUpdateTheValueInTheDatabase()
        {
            var dbfile = Path.Combine(RootPath, Guid.NewGuid().ToString("N") + ".db3");

            DatabaseHelper.CreateDatabase(dbfile);

            var db = new DatabaseHelper();
            var counter = new Counter
            {
                Name = "TestCounter",
                Description = "A test counter"
            };

            counter.Id.Should().Be(0);

            var res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter);
                return 0;
            }).Result;

            counter.Description = "New description";

            res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter);
                return 0;
            }).Result;

            var newCounter = Task.Run(async () => (await db.GetAllCountersAsync()).Single()).Result;

            newCounter.Should().NotBeSameAs(counter);
            newCounter.Id.Should().Be(counter.Id);
            newCounter.Name.Should().Be(counter.Name);
            newCounter.Description.Should().Be(counter.Description);
        }

        [Test]
        public void IncrementCounterShouldIncrementTheCounterValueInTheDatabase()
        {
            var dbfile = Path.Combine(RootPath, Guid.NewGuid().ToString("N") + ".db3");

            DatabaseHelper.CreateDatabase(dbfile);

            var db = new DatabaseHelper();
            var counter = new Counter
            {
                Name = "TestCounter",
                Description = "A test counter"
            };

            counter.Value.Should().Be(0);

            var res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter);
                return 0;
            }).Result;
            
            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter);
                return 0;
            }).Result;

            counter.Value.Should().Be(1);
            
            var newCounter = Task.Run(async () => (await db.GetAllCountersAsync()).Single()).Result;

            newCounter.Should().NotBeSameAs(counter);
            newCounter.Value.Should().Be(counter.Value);
        }

        [Test]
        public void IncrementingCounterStoresTheHistoryOfTheValuesInTheDatabase()
        {
            var dbfile = Path.Combine(RootPath, Guid.NewGuid().ToString("N") + ".db3");

            DatabaseHelper.CreateDatabase(dbfile);

            var db = new DatabaseHelper();
            var counter = new Counter
            {
                Name = "TestCounter",
                Description = "A test counter"
            };
            
            var res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter);
                return 0;
            }).Result;

            var now = DateTime.UtcNow;

            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter);
                return 0;
            }).Result;

            Thread.Sleep(2000);

            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter);
                return 0;
            }).Result;

            Thread.Sleep(2000);

            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter);
                return 0;
            }).Result;

            Thread.Sleep(2000);

            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter);
                return 0;
            }).Result;

            var history = Task.Run(async () => await db.GetCounterHistory(counter.Id)).Result.ToList();
            history.Count.Should().Be(4);
            history[0].IncrementDateTimeUtc.Should().BeOnOrAfter(now);
            history[1].IncrementDateTimeUtc.Should().BeOnOrAfter(now.AddSeconds(2));
            history[2].IncrementDateTimeUtc.Should().BeOnOrAfter(now.AddSeconds(4));
            history[3].IncrementDateTimeUtc.Should().BeOnOrAfter(now.AddSeconds(6));
        }

        [Test]
        public void GettingAllTheHistoryForACounterJustGetsTheHistoryForThatCounter()
        {
            var dbfile = Path.Combine(RootPath, Guid.NewGuid().ToString("N") + ".db3");

            DatabaseHelper.CreateDatabase(dbfile);

            var db = new DatabaseHelper();
            var counter1 = new Counter
            {
                Name = "Counter1",
                Description = "A test counter"
            };

            var counter2 = new Counter
            {
                Name = "Counter2",
                Description = "A second test counter"
            };

            var now = DateTime.UtcNow;

            var res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter1);
                return 0;
            }).Result;

            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter1);
                return 0;
            }).Result;

            res = Task.Run(async () =>
            {
                await db.AddOrUpdateCounterAsync(counter2);
                return 0;
            }).Result;

            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter2);
                return 0;
            }).Result;

            res = Task.Run(async () =>
            {
                await db.IncrementCounterAsync(counter2);
                return 0;
            }).Result;

            var history = Task.Run(async () => await db.GetCounterHistory(counter1.Id)).Result.ToList();

            history.Count.Should().Be(1);
            history[0].CounterId.Should().Be(counter1.Id);
            history[0].IncrementDateTimeUtc.Should().BeOnOrAfter(now).And.BeBefore(DateTime.UtcNow);
        }
    }
}
