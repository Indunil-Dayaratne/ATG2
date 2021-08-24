using System.Threading.Tasks;
using ATG.CodeTest.Models;

namespace ATG.CodeTest.DataAccess
{
    public class FailoverLotRepository : IFailoverLotRepository
    {
        public async Task<Lot> GetLotAsync(int id)
        {
            return await Task.FromResult(new Lot { Id = id });
        }
    }
}
