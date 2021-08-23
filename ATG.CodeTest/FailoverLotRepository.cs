using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATG.CodeTest
{
    public interface IFailoverLotRepository
    {
        Task<Lot> GetLotAsync(int id);
    }

    public class FailoverLotRepository : IFailoverLotRepository
    {
        public async Task<Lot> GetLotAsync(int id)
        {
            return await Task.FromResult(new Lot { Id = id });
        }
    }
}
