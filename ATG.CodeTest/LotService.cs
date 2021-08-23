using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public LotService(bool isFailoverModeEnabled, int maxFailedRequests,
            IFailoverLotRepository failoverLotRepository, IArchivedRepository archivedRepository,
            ILotRepository lotRepository, IFailoverLotEntryDataLoader failoverLotEntryDataLoader,
            ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _isFailoverModeEnabled = isFailoverModeEnabled;
            _maxFailedRequests = maxFailedRequests;
            _failoverLotRepository = failoverLotRepository;
            _archivedRepository = archivedRepository;
            _lotRepository = lotRepository;
            _failoverLotEntryDataLoader = failoverLotEntryDataLoader;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public async Task<Lot> GetLot(int id, bool isLotArchived)
        {
            Lot lot = null;

            var failoverLots = await _failoverLotEntryDataLoader.GetFailOverLotEntriesAsync();
            var failedRequests = failoverLots.Count(failoverLotsEntry => failoverLotsEntry.DateTime > _currentDateTimeProvider.GetCurrentDateTime().AddMinutes(10));
            if ((failedRequests > _maxFailedRequests) && _isFailoverModeEnabled)
            {
                lot = await _failoverLotRepository.GetLotAsync(id);
            }

            if (lot != null)
            {
                return lot.IsArchived ? await _archivedRepository.GetLotAsync(id) : lot;
            }

            if (isLotArchived)
            {
                return await _archivedRepository.GetLotAsync(id);
            }

            return await _lotRepository.GetLotAsync(id);
        }
    }
}
