using System.Threading.Tasks;

namespace ATG.CodeTest
{
    public interface ILotRepository
    { 
        Task<Lot> GetLotAsync(int id);
    }

    public class LotRepository : ILotRepository
    {
        public async Task<Lot> GetLotAsync(int id)
        {
            return await Task.FromResult(new Lot { Id = id });
        }
    }
}
