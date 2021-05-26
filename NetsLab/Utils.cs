using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsTest2
{
    class Utils
    {
        public static int CheckSum(BitArray arr)
        {
            int k = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == true)
                {
                    k += 1;
                }
            }
            return k;
        }

        public static BitArray DecimalToBinary(int decimalNumber)
        {
            BitArray binaryNumber = new BitArray(8);
            for (int i = 0; i < binaryNumber.Length; i++)
            {
                if (decimalNumber % 2 == 1)
                {
                    binaryNumber[(binaryNumber.Length - 1) - i] = true;
                }
                decimalNumber /= 2;
            }

            return binaryNumber;
        }

        public static BitArray SplitMessage(BitArray recievedMessage, int index)
        {

            BitArray partOfFrame = new BitArray(50);

            if (recievedMessage.Length > 50)
            {
                for (int i = index; i < index + 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        partOfFrame[j] = recievedMessage[i];
                    }
                }
            }

            return partOfFrame;
        }
    }
}