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
        
        public Producer(List<DataItem> data)
        {
            _poller = new Timer(x => Poll(), null, TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
            _data = data;
        }

        private void Poll()
        {
            try
            {
                _data.Add(new DataItem(RandomStringGenerator.Generate(6)));
            }
            finally
            {
                _poller.Change(TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
            }
        }

        public void Dispose()
        {
            _poller?.Dispose();
        }
    }
}