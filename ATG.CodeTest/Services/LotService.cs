using System;
using System.Linq;
using System.Threading.Tasks;
using ATG.CodeTest.DataAccess;
using ATG.CodeTest.Models;
using ATG.CodeTest.Utils;

namespace ATG.CodeTest.Services
{
    public class LotService : ILotService
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
            _failoverLotRepository = failoverLotRepository ?? throw new ArgumentNullException(nameof(failoverLotRepository));
            _archivedRepository = archivedRepository ?? throw new ArgumentNullException(nameof(archivedRepository));
            _lotRepository = lotRepository ?? throw new ArgumentNullException(nameof(lotRepository));
            _failoverLotEntryDataLoader = failoverLotEntryDataLoader ?? throw new ArgumentNullException(nameof(failoverLotEntryDataLoader));
            _currentDateTimeProvider = currentDateTimeProvider ?? throw new ArgumentNullException(nameof(currentDateTimeProvider));
        }

        public async Task<Lot> GetLotAsync(int id, bool isLotArchived)
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

            lot = await _lotRepository.GetLotAsync(id);

            if (lot != null)
            {
                return lot.IsArchived ? await _archivedRepository.GetLotAsync(id) : lot;
            }

            return lot;
        }
    }
}
