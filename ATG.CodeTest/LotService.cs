using System;
using System.Collections.Generic;
using System.Linq;

namespace ATG.CodeTest
{
    public class LotService
    {
        private readonly bool _isFailoverModeEnabled;
        private readonly int _maxFailedRequests;
        private readonly IFailoverLotRepository _failoverLotRepository;
        private readonly IArchivedRepository _archivedRepository;
        private readonly ILotRepository _lotRepository;
        private readonly IFailoverLotEntryDataLoader _failoverLotEntryDataLoader;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public LotService(bool isFailoverModeEnabled, int maxFailedRequests, IFailoverLotRepository failoverLotRepository, IArchivedRepository archivedRepository, ILotRepository lotRepository, IFailoverLotEntryDataLoader failoverLotEntryDataLoader, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _isFailoverModeEnabled = isFailoverModeEnabled;
            _maxFailedRequests = maxFailedRequests;
            _failoverLotRepository = failoverLotRepository;
            _archivedRepository = archivedRepository;
            _lotRepository = lotRepository;
            _failoverLotEntryDataLoader = failoverLotEntryDataLoader;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public Lot GetLot(int id, bool isLotArchived)
        {
            Lot lot = null;

            var failoverLots = _failoverLotEntryDataLoader.GetFailOverLotEntries();
            var failedRequests = failoverLots.Count(failoverLotsEntry => failoverLotsEntry.DateTime > _currentDateTimeProvider.GetCurrentDateTime().AddMinutes(10));
            if ((failedRequests > _maxFailedRequests) && _isFailoverModeEnabled)
            {
                lot = _failoverLotRepository.GetLot(id);
            }

            if (lot != null)
            {
                return lot.IsArchived ? _archivedRepository.GetLot(id) : lot;
            }

            if (isLotArchived)
            {
                return _archivedRepository.GetLot(id);
            }

            return _lotRepository.GetLot(id);
        }
    }
}
