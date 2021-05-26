using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsTest2
{
    class Frame
    {
        static Random rand = new Random();
        private static BitArray _sendMessage;
        public static int checkSum;

        public static BitArray CreateFrame()
        {
            //переменная длина
            int length = rand.Next(50, 200);
            _sendMessage = new BitArray(length);

            //длина кадра в заголовке кадра
            for (int i = 0; i < 6; i++)
            {
                _sendMessage[i] = Utils.DecimalToBinary(length)[i];
            }

            //данные
            for (int i = 6; i < length; i++)
            {
                if (i % 2 == 0)
                    _sendMessage[i] = true;
                else
                    _sendMessage[i] = false;
            }

            return _sendMessage;
        }

        public static int getFramesCheckSum()
        {
            checkSum = Utils.CheckSum(_sendMessage);
            return checkSum;
        }
    }
}