using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TestHarness1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Testing Monitor-Based Blocking Queue");
            Console.Write("\n ======================================");

            SWTools.BlockingQueue<string> q = new SWTools.BlockingQueue<string>();
            Thread t = new Thread(() => {
                string msg;
                while (true)
                {
                    msg = q.deQ(); Console.Write("\n  child thread received {0}", msg);
                    if (msg == "quit") break;
                }
            });
            t.Start();
            string sendMsg = "msg #";
            for (int i = 0; i < 20; ++i)
            {
                string temp = sendMsg + i.ToString();
                Console.Write("\n  main thread sending {0}", temp);
                q.enQ(temp);
            }
            q.enQ("quit");
            t.Join();
            Console.Write("\n\n");
        }
    }
}
