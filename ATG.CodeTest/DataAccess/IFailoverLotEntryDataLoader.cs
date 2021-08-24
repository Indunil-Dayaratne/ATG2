using System.Collections.Generic;
using System.Threading.Tasks;
using ATG.CodeTest.Models;

namespace ATG.CodeTest.DataAccess
{
    public interface IFailoverLotEntryDataLoader
    {
        Task<List<FailoverLots>> GetFailOverLotEntriesAsync();
    }
}