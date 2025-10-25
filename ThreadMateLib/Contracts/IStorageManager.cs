using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadMateLib.Domain;

namespace ThreadMateLib.Contracts
{
    public interface IStorageManager
    {
        void StoreThreadMateData(List<ThreadMateProcess> processes);

        List<ThreadMateProcess> RetrieveThreadMateData();
    }
}
