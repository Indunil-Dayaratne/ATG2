using System;

namespace ATG.CodeTest.Utils
{
    public class CurrentDateTimeProvider : ICurrentDateTimeProvider
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}