using System.Collections.Generic;
using System.Threading.Tasks;
using ATG.CodeTest.Models;

namespace ATG.CodeTest.DataAccess
{
    public class FailoverLotEntryDataLoader : IFailoverLotEntryDataLoader
    {
        public async Task<List<FailoverLots>> GetFailOverLotEntriesAsync()
        {
            // return all from fail entries from database
            return await Task.FromResult(new List<FailoverLots>());
        }
    }
}
