using System;
using SQLite;

namespace StupendousCounter.Core
{
    public class CounterIncrementHistory
    {
        [Indexed]
        public int CounterId { get; set; }
        public DateTime IncrementDateTimeUtc { get; set; }
    }
}
