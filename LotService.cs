using System;
using System.Collections.Generic;
using System.Linq;

namespace ATG.CodeTest
{
    public class LotService
    {
        public Lot GetLot(int id, bool isLotArchived)
        {
            bool isFailoverModeEnabled = true;
            int MaxFailedRequests = 50;
            Lot lot = null;

            var failoverLots = GetFailOverLotEntries();
            var failedRequests = failoverLots.Where(failoverLotsEntry => failoverLotsEntry.DateTime > DateTime.Now.AddMinutes(10)).Count();
            if ((failedRequests > MaxFailedRequests) && isFailoverModeEnabled)
            {
                lot = new FailoverLotRepository().GetLot(id);
            }

            if (lot.IsArchived && isLotArchived)
            {
                return new ArchivedRepository().GetLot(id);
            }
            else
            {
                return new LotRepository().LoadCustomer();
            }
        }

        public List<FailoverLots> GetFailOverLotEntries()
        {
            // return all from fail entries from database
            return new List<FailoverLots>();
        }
    }
}
