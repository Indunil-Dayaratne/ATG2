using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATG.CodeTest.DataAccess;
using ATG.CodeTest.Models;
using ATG.CodeTest.Services;
using ATG.CodeTest.Utils;
using Moq;
using Xunit;

namespace ATG.CodeTest.Tests.Services
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
            const int id = 123;

            var mockFailoverLotRepository = new Mock<IFailoverLotRepository>();
            var mockArchivedRepository = new Mock<IArchivedRepository>();
            var mockLotRepository = new Mock<ILotRepository>();
            var mockFailoverLotEntryDataLoader = new Mock<IFailoverLotEntryDataLoader>();
            var mockCurrentDateTimeProvider = new Mock<ICurrentDateTimeProvider>();

            var service = new LotService(true, 2, mockFailoverLotRepository.Object, mockArchivedRepository.Object,
                mockLotRepository.Object, mockFailoverLotEntryDataLoader.Object, mockCurrentDateTimeProvider.Object);

            mockCurrentDateTimeProvider.Setup(x => x.GetCurrentDateTime()).Returns(DateTime.Now.AddMinutes(-30));
            mockFailoverLotEntryDataLoader.Setup(x => x.GetFailOverLotEntriesAsync())
                .ReturnsAsync(new List<FailoverLots>
                {
                    new FailoverLots {DateTime = DateTime.Now}, 
                    new FailoverLots {DateTime = DateTime.Now},
                    new FailoverLots {DateTime = DateTime.Now}
                });

            mockFailoverLotRepository.Setup(x => x.GetLotAsync(id)).ReturnsAsync(new Lot {Id = id});

            await service.GetLotAsync(id, false);

            mockArchivedRepository.Verify(x => x.GetLotAsync(id), Times.Never);
            mockFailoverLotRepository.Verify(x => x.GetLotAsync(id), Times.Once);
            mockLotRepository.Verify(x => x.GetLotAsync(id), Times.Never);
        }

        [Fact]
        public async Task GetLotAsyncLoadsDataFromBothFailoverAndArchivedDataStores()
        {
            const int id = 123;

            var mockFailoverLotRepository = new Mock<IFailoverLotRepository>();
            var mockArchivedRepository = new Mock<IArchivedRepository>();
            var mockLotRepository = new Mock<ILotRepository>();
            var mockFailoverLotEntryDataLoader = new Mock<IFailoverLotEntryDataLoader>();
            var mockCurrentDateTimeProvider = new Mock<ICurrentDateTimeProvider>();

            var service = new LotService(true, 2, mockFailoverLotRepository.Object, mockArchivedRepository.Object,
                mockLotRepository.Object, mockFailoverLotEntryDataLoader.Object, mockCurrentDateTimeProvider.Object);

            mockCurrentDateTimeProvider.Setup(x => x.GetCurrentDateTime()).Returns(DateTime.Now.AddMinutes(-30));
            mockFailoverLotEntryDataLoader.Setup(x => x.GetFailOverLotEntriesAsync())
                .ReturnsAsync(new List<FailoverLots>
                {
                    new FailoverLots {DateTime = DateTime.Now},
                    new FailoverLots {DateTime = DateTime.Now},
                    new FailoverLots {DateTime = DateTime.Now}
                });

            mockFailoverLotRepository.Setup(x => x.GetLotAsync(id)).ReturnsAsync(new Lot { Id = id, IsArchived = true});

            await service.GetLotAsync(id, false);

            mockArchivedRepository.Verify(x => x.GetLotAsync(id), Times.Once);
            mockFailoverLotRepository.Verify(x => x.GetLotAsync(id), Times.Once);
            mockLotRepository.Verify(x => x.GetLotAsync(id), Times.Never);
        }

        [Fact]
        public async Task GetLotAsyncLoadsDataFromMainDataStore()
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

            await service.GetLotAsync(id, false);

            mockArchivedRepository.Verify(x => x.GetLotAsync(id), Times.Never);
            mockFailoverLotRepository.Verify(x => x.GetLotAsync(id), Times.Never);
            mockLotRepository.Verify(x => x.GetLotAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetLotAsyncLoadsDataFromBothMainAndArchivedDataStores()
        {
            const int id = 123;

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
            mockLotRepository.Setup(x => x.GetLotAsync(id)).ReturnsAsync(new Lot { Id = id, IsArchived = true });

            await service.GetLotAsync(id, false);

            mockArchivedRepository.Verify(x => x.GetLotAsync(id), Times.Once);
            mockFailoverLotRepository.Verify(x => x.GetLotAsync(id), Times.Never);
            mockLotRepository.Verify(x => x.GetLotAsync(id), Times.Once);
        }
    }
}
