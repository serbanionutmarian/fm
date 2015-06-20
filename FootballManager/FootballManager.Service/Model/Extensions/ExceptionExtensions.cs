using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Extensions
{
    public static class ExceptionExtensions
    {
        public static Log ToLog(this Exception ex)
        {
            return new Log()
            {
                CreatedAt = DateTime.Now,
                Message = ex.Message,
                Source = ex.Source,
                StackTrace = ex.StackTrace
            };
        }
    }
}
