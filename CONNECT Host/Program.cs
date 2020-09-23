using System;
using System.Threading;

namespace CONNECTServerSide
{
    class Program
    {
        private static bool isRunning = false;

        static void Main()
        {
            Console.Title = "Server Side";
            isRunning = true;
            bool autoStart = true;
            
            if (!autoStart)
            {
                int _numberMaxClients, _portNumber;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Insert the number of max clients:");
                        _numberMaxClients = int.Parse(Console.ReadLine());
                        if (_numberMaxClients > 0)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("The number must be a positive integer grater than 0.");
                    }
                }

                while (true) 
                {
                    try
                    {
                        Console.WriteLine("Insert the port to use for the server:");
                        _portNumber = int.Parse(Console.ReadLine());
                        if (_portNumber >= 0 && _portNumber <= 65535)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("The port number must be an integer between 0 and 65535. Check this link for more information: https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers");
                    }
                }

                Thread mainThread = new Thread(new ThreadStart(MainThread));
                mainThread.Start();
            
                Server.Start(_numberMaxClients, _portNumber);
            }
            else
            {
                Thread mainThread = new Thread(new ThreadStart(MainThread));
                mainThread.Start();

                Server.Start(50, 26950);
            }
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICK_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    AppLogic.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
