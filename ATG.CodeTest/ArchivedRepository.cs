using System;
using System.Collections.Generic;
using System.Text;

namespace ATG.CodeTest
{
    public interface IArchivedRepository
    {
        Lot GetLot(int id);
    }

    public class ArchivedRepository : IArchivedRepository
    {
        public Lot GetLot(int id)
        {
            return new Lot { Id = id };
        }
    }
}
