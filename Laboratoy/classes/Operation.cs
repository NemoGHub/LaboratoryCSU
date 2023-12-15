using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratoy.classes
{
    public class Operation : Token
    {
        public char operation;


        public char GetToken()
        {
            return operation;
        }
        public void SetToken(char token)
        {
            operation = token;
        }
    }
}
