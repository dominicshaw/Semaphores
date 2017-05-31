using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            _producer = new Producer(data);
            _consumer = new Consumer(repository, Data);
        }

        public void Dispose()
        {
            _producer?.Dispose();
            _consumer?.Dispose();
        }
    }
}