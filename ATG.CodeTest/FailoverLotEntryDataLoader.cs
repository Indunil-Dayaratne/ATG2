using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATG.CodeTest
{
    public interface IFailoverLotEntryDataLoader
    {
        Task<List<FailoverLots>> GetFailOverLotEntriesAsync();
    }

    public class FailoverLotEntryDataLoader : IFailoverLotEntryDataLoader
    {
        public async Task<List<FailoverLots>> GetFailOverLotEntriesAsync()
        {
            // return all from fail entries from database
            return await Task.FromResult(new List<FailoverLots>());
        }
    }
}
