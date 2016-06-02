using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StupendousCounter.Core
{
    public interface IDatabaseHelper
    {
        Task DeleteCounterAsync(Counter counter);
        Task AddOrUpdateCounterAsync(Counter counter);
        Task IncrementCounterAsync(Counter counter);
        Task<IEnumerable<Counter>> GetAllCountersAsync();
        Task<IEnumerable<CounterIncrementHistory>> GetCounterHistory(int counterId);

        event EventHandler CountersChanged;
    }
}
