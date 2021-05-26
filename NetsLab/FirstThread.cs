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
    public class FirstThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private BitArray _receivedMessage;
        private PostToSecondWT _post;
        private Random rnd = new Random();
        private BitArray _receipt;

        public static BitArray _sendMessage;

        public FirstThread(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
        }
        public void FirstThreadMain(object obj)
        {
            _post = (PostToSecondWT)obj;
            ConsoleHelper.WriteToConsole("1 поток", "Начинаю работу.Готовлю данные для передачи.");
            //создание куска данных

            _sendMessage = Frame.CreateFrame();

            //отправка данных
            Stopwatch recieptTime = new Stopwatch();
            _post(_sendMessage);
            _sendSemaphore.Release();

            Console.WriteLine("1 поток, контрольная сумма: " + Utils.CheckSum(_sendMessage));

            ConsoleHelper.WriteToConsole("1 поток", "Данные переданы");

            //ждем квитанцию от второго потока
            recieptTime.Start();
            _receiveSemaphore.WaitOne();
            recieptTime.Stop();

            //таймаут на получение квитанции
            TimeSpan recieptTimeSpan = recieptTime.Elapsed;

            if (recieptTimeSpan.Milliseconds < 10)
            {
                ConsoleHelper.WriteToConsole("1 поток", "Квитанция получена");
            }
            else
            {
                ConsoleHelper.WriteToConsole("1 поток", "Квитанция не получена, отправляю данные повторно");
                _post(_sendMessage);
                _sendSemaphore.Release();
            }

            ConsoleHelper.WriteToConsole("1 поток", "Жду передачи данных");

            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsole("1 поток", "Данные получены.");
            ConsoleHelper.WriteToConsoleArray("1 поток", _receivedMessage);

            Console.WriteLine("1 поток: Контрольная сумма: " + Utils.CheckSum(_receivedMessage));

            if (Utils.CheckSum(_receivedMessage) == Utils.CheckSum(SecondThread._sendMessage))
            {
                Console.WriteLine("1 поток: Контрольная сумма совпала");
            }
            else
            {
                Console.WriteLine("Данные повреждены");
            }

            ConsoleHelper.WriteToConsole("1 поток", "Подготавливаю квитанцию");

            _receipt = Reciept.CreateReciept();
            ConsoleHelper.WriteToConsole("1 поток", "Отправляю квитанцию");
            _post(_receipt);
            _sendSemaphore.Release();

            ConsoleHelper.WriteToConsole("1 поток", "Завершаю работу.");
        }
        public void ReceiveData(BitArray array)
        {
            _receivedMessage = array;
        }
    }
}
