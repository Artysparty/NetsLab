using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadsEdu;

namespace ThreadsTest2
{
    public class SecondThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private BitArray _receivedMessage;
        private PostToFirstWT _post;
        private BitArray _receipt;

        public static BitArray _sendMessage;

        public SecondThread(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
        }
        public void SecondThreadMain(Object obj)
        {
            _post = (PostToFirstWT)obj;

            ConsoleHelper.WriteToConsole("2 поток", "Начинаю работу.Жду передачи данных.");

            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsole("2 поток", "Данные полученны");
            ConsoleHelper.WriteToConsoleArray("2 поток", _receivedMessage);

            //разбиваем сообщение
            //BitArray[] masMessage = new BitArray[_receivedMessage.Length/50];
            //Console.WriteLine();
            //for (int i = 0; i < _receivedMessage.Length/50; i++)
            //{
            //    masMessage[i] = Utils.SplitMessage(_receivedMessage, i);
            //    i += 50;
            //    ConsoleHelper.WriteBitArray(masMessage[i]);
            //    Console.WriteLine();
            //}
            //Console.WriteLine();


            Console.WriteLine("2 поток: Контрольная сумма: " + Utils.CheckSum(_receivedMessage));

            if (Utils.CheckSum(_receivedMessage) == Utils.CheckSum(FirstThread._sendMessage))
            {
                Console.WriteLine("Контрольная сумма совпала");
            }
            else
            {
                Console.WriteLine("Данные повреждены");
            }

            //Отправка квитанции
            ConsoleHelper.WriteToConsole("2 поток", "Подготавливаю квитанцию");

            _receipt = Reciept.CreateReciept();

            ConsoleHelper.WriteToConsole("2 поток", "Отправляю квитанцию");

            _post(_receipt);
            _sendSemaphore.Release();

            ConsoleHelper.WriteToConsole("2 поток", "Подготавливаю данные.");

            //создание данных
            _sendMessage = Frame.CreateFrame();

            Stopwatch recieptTime = new Stopwatch();

            //отправление данных
            _post(_sendMessage);
            _sendSemaphore.Release();

            Console.WriteLine("2 поток, контрольная сумма: " + Utils.CheckSum(_sendMessage));

            ConsoleHelper.WriteToConsole("2 поток", "Данные переданы");

            //ждем квитанцию от 1 потока
            ConsoleHelper.WriteToConsole("2 поток", "Жду получения квитанции");

            recieptTime.Start();
            _receiveSemaphore.WaitOne();
            recieptTime.Stop();

            TimeSpan recieptTimeSpan = recieptTime.Elapsed;

            if (recieptTimeSpan.Milliseconds < 10)
            {
                ConsoleHelper.WriteToConsole("2 поток", "Квитанция получена");

                ConsoleHelper.WriteToConsole("2 поток", "Заканчиваю работу.");
            }
            else
            {
                ConsoleHelper.WriteToConsole("2 поток", "Квитанция не получена, отправляю данные повторно");
                _post(_sendMessage);
                _sendSemaphore.Release();
            }
        }

        public void ReceiveData(BitArray array)
        {
            _receivedMessage = array;
        }
    }
}
