using System;
using System.Collections.Generic;
using System.Text;

namespace ATG.CodeTest
{
    public interface IFailoverLotEntryDataLoader
    {
        List<FailoverLots> GetFailOverLotEntries();
    }

    public class FailoverLotEntryDataLoader : IFailoverLotEntryDataLoader
    {
        public List<FailoverLots> GetFailOverLotEntries()
        {
            // return all from fail entries from database
            return new List<FailoverLots>();
        }
    }
}
