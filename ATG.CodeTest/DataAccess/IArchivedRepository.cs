using System.Threading.Tasks;
using ATG.CodeTest.Models;

namespace ATG.CodeTest.DataAccess
{
    public interface IArchivedRepository
    {
        Task<Lot> GetLotAsync(int id);
    }
}