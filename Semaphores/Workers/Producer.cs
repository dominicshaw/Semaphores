using System;
using System.Collections.Generic;
using System.Threading;
using Semaphores.Models;

namespace Semaphores.Workers
{
    public class Producer : IDisposable
    {
        private readonly Timer _poller;
        private readonly List<DataItem> _data;
        private readonly SemaphoreSlim _semaphore;

        public Producer(List<DataItem> data, SemaphoreSlim semaphore)
        {
            _poller = new Timer(x => Poll(), null, TimeSpan.FromMilliseconds(1000), Timeout.InfiniteTimeSpan);
            _data = data;
            _semaphore = semaphore;
        }

        private void Poll()
        {
            try
            {
                _semaphore.Wait();
                _data.Add(new DataItem(RandomStringGenerator.Generate(6)));
            }
            finally
            {
                _poller.Change(TimeSpan.FromMilliseconds(100), Timeout.InfiniteTimeSpan);
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _poller?.Dispose();
        }
    }
}