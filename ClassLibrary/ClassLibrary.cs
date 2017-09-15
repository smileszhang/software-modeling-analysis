///////////////////////////////////////////////////////////////////////////
// DemoClassLibrary.cs - will be loaded and used by child AppDomain      //
//                                                                       //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2004       //
///////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TestHarness1
{
    public class Execute
    {
        /* 
         * This method wraps code passed by Action delegate in try - catch block and executes it. 
         */
        public static void doExecute(System.Action a)
        {
            // Action is equivalent to: public delegate void Action()
            try
            {
                a.Invoke();
            }
            catch (Exception ex)
            {
                Console.Write("\n  Exception caught in child domain: {0}", ex.Message);
            }
        }
    }
    ///////////////////////////////////////////////////////////////////
    // hello1 class method say() executes code normally
    //
    public class hello1 : MarshalByRefObject, IHello
    {
        AppDomain ad = null;

        public hello1()
        {

            // Console.Write("\n  Constructing hello1 in AppDomain {0}", ad.FriendlyName);
        }

        public virtual int say(List<string> names)  //return 1 success.return 0 fail. return 2 other situation
        {
            // Execute.doExecute(() => { Console.Write("\n  hello1 - normal execution"); });
            ad = AppDomain.CurrentDomain;

            //string path = "../../../Tests";
            //string path = "C:\\Users\\shiyang\\Documents\\Visual Studio 2015\\Projects\\ConsoleApplication3\\Tests";
            TestHarness th = new TestHarness();
            if (th.LoadTests(names))
            {
                int r = th.run();
                if (r == 1)
                {
                    return 1;
                }
                else if (r == 0)
                {
                    return 0;
                }

            }
            else
                Console.Write("\n  couldn't load tests");

            Console.Write("\n\n");
            return 2;
        }

    }

}