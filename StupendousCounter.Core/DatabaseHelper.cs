using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace StupendousCounter.Core
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private static string _dbPath;

        public static void CreateDatabase(string dbPath)
        {
            _dbPath = dbPath;

            using (var connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.Create|SQLiteOpenFlags.ReadWrite))
            {
                connection.CreateTable<Counter>();
                connection.CreateTable<CounterIncrementHistory>();
            }
        }
        
        public async Task AddOrUpdateCounterAsync(Counter counter)
        {
            var connection = new SQLiteAsyncConnection(_dbPath);
            if (counter.Id == 0)
                await connection.InsertAsync(counter);
            else
                await connection.InsertOrReplaceAsync(counter);

            OnCountersChanged();
        }

        public async Task IncrementCounterAsync(Counter counter)
        {
            var connection = new SQLiteAsyncConnection(_dbPath);

            counter.Value++;
            await AddOrUpdateCounterAsync(counter);
            var history = new CounterIncrementHistory
            {
                CounterId = counter.Id,
                IncrementDateTimeUtc = DateTime.UtcNow
            };

            await connection.InsertAsync(history);
        }

        public async Task<IEnumerable<Counter>> GetAllCountersAsync()
        {
            var connection = new SQLiteAsyncConnection(_dbPath);
            return await connection.Table<Counter>().ToListAsync();
        }

        public async Task<IEnumerable<CounterIncrementHistory>> GetCounterHistory(int counterId)
        {
            var connection = new SQLiteAsyncConnection(_dbPath);
            return await connection.Table<CounterIncrementHistory>().Where(c => c.CounterId == counterId).ToListAsync();
        }

        public event EventHandler CountersChanged;

        private void OnCountersChanged()
        {
            CountersChanged?.Invoke(this, new EventArgs());
        }
    }
}