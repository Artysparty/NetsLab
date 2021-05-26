using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsTest2
{
    class Reciept
    {
        private static BitArray _receipt;

        public static BitArray CreateReciept()
        {
            _receipt = new BitArray(1);
            _receipt[0] = true;
            return _receipt;
        }
    }
}