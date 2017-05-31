using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Semaphores.Models;
using Semaphores.Workers;

namespace Semaphores.ViewModels
{
    public class MainViewModel : IDisposable
    {
        private readonly Producer _producer;
        private readonly Consumer _consumer;

        public ObservableCollection<DataItem> Data { get; } = new ObservableCollection<DataItem>();

        public MainViewModel()
        {
            var data = new List<DataItem>();

            var repository = new Repository(data);

            var semaphore = new SemaphoreSlim(1, 1);

            _producer = new Producer(data, semaphore);
            _consumer = new Consumer(repository, Data, semaphore);
        }

        public void Dispose()
        {
            _producer?.Dispose();
            _consumer?.Dispose();
        }
    }
}