using System.Collections.Generic;
using Semaphores.Models;

namespace Semaphores.Workers
{
    public class Repository
    {
        private readonly List<DataItem> _data;

        public Repository(List<DataItem> data)
        {
            _data = data;
        }

        public List<DataItem> GetData()
        {
            return _data;
        }
    }
}