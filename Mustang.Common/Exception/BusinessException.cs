using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mustang.Common
{
    public class BusinessException : ApplicationException
    {
        public BusinessException(string message) : base(message)
        {

        }

        
    }
}
