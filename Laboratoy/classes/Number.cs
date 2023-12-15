using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratoy.classes
{
    public class Number : Token
    {
        public double number;

        public double GetToken()
        {
            return number;
        }

        public void SetToken(double token)
        {
            number = token;
        }
    }
}
