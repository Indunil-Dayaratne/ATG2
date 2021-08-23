using System;
using System.Collections.Generic;
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
        public async Task GetLotAsyncLoadsDataFromArchivedDataStore()
        {
            var mockFailoverLotRepository = new Mock<IFailoverLotRepository>();
            var mockArchivedRepository = new Mock<IArchivedRepository>();
            var mockLotRepository = new Mock<ILotRepository>();
            var mockFailoverLotEntryDataLoader = new Mock<IFailoverLotEntryDataLoader>();
            var mockCurrentDateTimeProvider = new Mock<ICurrentDateTimeProvider>();

            var service = new LotService(true, 50, mockFailoverLotRepository.Object, mockArchivedRepository.Object,
                mockLotRepository.Object, mockFailoverLotEntryDataLoader.Object, mockCurrentDateTimeProvider.Object);

            mockFailoverLotEntryDataLoader.Setup(x => x.GetFailOverLotEntriesAsync())
                .ReturnsAsync(new List<FailoverLots>());

            mockCurrentDateTimeProvider.Setup(x => x.GetCurrentDateTime()).Returns(DateTime.UtcNow);

            const int id = 123;

            await service.GetLotAsync(id, true);

            mockArchivedRepository.Verify(x=>x.GetLotAsync(id), Times.Once);
            mockFailoverLotRepository.Verify(x=>x.GetLotAsync(id), Times.Never);
            mockLotRepository.Verify(x=>x.GetLotAsync(id), Times.Never);
        }

        [Fact]
        public async Task GetLotAsyncLoadsDataFromFailoverDataStore()
        {

        }

        [Fact]
        public async Task GetLotAsyncLoadsDataFromBothFailoverAndArchivedDataStores()
        {

        }

        [Fact]
        public async Task GetLotAsyncLoadsDataFromMainDataStore()
        {

        }

        [Fact]
        public async Task GetLotAsyncLoadsDataFromBothMainAndArchivedDataStores()
        {

        }
    }
}
