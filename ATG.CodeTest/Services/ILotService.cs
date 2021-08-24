using System.Threading.Tasks;
using ATG.CodeTest.Models;

namespace ATG.CodeTest.Services
{
    public interface ILotService
    {
        Task<Lot> GetLotAsync(int id, bool isLotArchived);
    }
}