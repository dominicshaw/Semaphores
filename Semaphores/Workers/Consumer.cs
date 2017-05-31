using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using Semaphores.Models;

namespace Semaphores.Workers
{
    public class Consumer : IDisposable
    {
        private readonly Repository _repository;
        private readonly Timer _poller;

        private readonly ObservableCollection<DataItem> _data;
        private readonly SemaphoreSlim _semaphore;

        public Consumer(Repository repository, ObservableCollection<DataItem> data, SemaphoreSlim semaphore)
        {
            _repository = repository;
            _data       = data;
            _semaphore  = semaphore;

            _poller     = new Timer(x => Poll(), null, TimeSpan.FromMilliseconds(1000), Timeout.InfiniteTimeSpan);
        }

        private void Poll()
        {
            try
            {
                _semaphore.Wait();

                var newData = _repository.GetData();

                foreach (var d in newData)
                {
                    if (_data.Contains(d))
                        continue;

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _data.Add(d);
                    }));
                }
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