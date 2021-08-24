using System.Threading.Tasks;
using ATG.CodeTest.Models;

namespace ATG.CodeTest.DataAccess
{
    public interface ILotRepository
    { 
        Task<Lot> GetLotAsync(int id);
    }
}