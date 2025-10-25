using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadMateLib.Contracts;

namespace ThreadMateLib.Domain
{
    public class FlatFileStorageManager : IStorageManager
    {
        public List<ThreadMateProcess> RetrieveThreadMateData()
        {
            throw new NotImplementedException();
        }

        public void StoreThreadMateData(List<ThreadMateProcess> processes)
        {
            throw new NotImplementedException();
        }
    }
}
