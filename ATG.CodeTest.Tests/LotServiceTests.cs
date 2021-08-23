using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ATG.CodeTest.Tests
{
    public class LotServiceTests
    {
        [Fact]
        public void ServiceCannotBeConstructedWithoutSupplyingRequiredDependencies()
        {
            var mockFailoverLotRepository = new Mock<IFailoverLotRepository>();
            var mockArchivedRepository = new Mock<IArchivedRepository>();
            var mockLotRepository = new Mock<ILotRepository>();
            var mockFailoverLotEntryDataLoader = new Mock<IFailoverLotEntryDataLoader>();
            var mockCurrentDateTimeProvider = new Mock<ICurrentDateTimeProvider>();

            Assert.Throws<ArgumentNullException>(() => new LotService(true, 50, null, mockArchivedRepository.Object,
                mockLotRepository.Object, mockFailoverLotEntryDataLoader.Object, mockCurrentDateTimeProvider.Object));
            Assert.Throws<ArgumentNullException>(() => new LotService(true, 50, mockFailoverLotRepository.Object, null,
                mockLotRepository.Object, mockFailoverLotEntryDataLoader.Object, mockCurrentDateTimeProvider.Object));
            Assert.Throws<ArgumentNullException>(() => new LotService(true, 50, mockFailoverLotRepository.Object, mockArchivedRepository.Object,
                null, mockFailoverLotEntryDataLoader.Object, mockCurrentDateTimeProvider.Object));
            Assert.Throws<ArgumentNullException>(() => new LotService(true, 50, mockFailoverLotRepository.Object, mockArchivedRepository.Object,
                mockLotRepository.Object, null, mockCurrentDateTimeProvider.Object));
            Assert.Throws<ArgumentNullException>(() => new LotService(true, 50, mockFailoverLotRepository.Object, mockArchivedRepository.Object,
                mockLotRepository.Object, mockFailoverLotEntryDataLoader.Object, null));
            var service = new LotService(true, 50, mockFailoverLotRepository.Object, mockArchivedRepository.Object,
                mockLotRepository.Object, mockFailoverLotEntryDataLoader.Object, mockCurrentDateTimeProvider.Object);

            Assert.NotNull(service);
        }

        [Fact]
        public async Task GetLotLoadsDataFromArchivedDataStore()
        {

        }

        [Fact]
        public async Task GetLotLoadsDataFromFailoverDataStore()
        {

        }

        [Fact]
        public async Task GetLotLoadsDataFromBothFailoverAndArchivedDataStores()
        {

        }

        [Fact]
        public async Task GetLotLoadsDataFromMainDataStore()
        {

        }

        [Fact]
        public async Task GetLotLoadsDataFromBothMainAndArchivedDataStores()
        {

        }
    }
}
