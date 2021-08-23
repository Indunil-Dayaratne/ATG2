using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATG.CodeTest
{
    public interface IArchivedRepository
    {
        Task<Lot> GetLotAsync(int id);
    }

    public class ArchivedRepository : IArchivedRepository
    {
        public async Task<Lot> GetLotAsync(int id)
        {
            return await Task.FromResult(new Lot { Id = id });
        }
    }
}
