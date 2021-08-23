using System;
using System.Collections.Generic;
using System.Text;

namespace ATG.CodeTest
{
    public interface ILotRepository
    {
        Lot GetLot(int id);
    }

    public class LotRepository : ILotRepository
    {
        public Lot GetLot(int id)
        {
            return new Lot { Id = id };
        }
    }
}
