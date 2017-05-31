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

        public Consumer(Repository repository, ObservableCollection<DataItem> data)
        {
            _repository = repository;
            _data = data;
            _poller = new Timer(x => Poll(), null, TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
        }

        private void Poll()
        {
            try
            {
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
                _poller.Change(TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
            }
        }

        public void Dispose()
        {
            _poller?.Dispose();
        }
    }
}