///////////////////////////////////////////////////////////////////////////
// AppDomainDemo.cs - Tests Error Handling                               //
//                                                                       //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2016       //
///////////////////////////////////////////////////////////////////////////
/*
*  Note: 
*    If this doesn't compile, that means you need to set the project properties
*    to use the current .Net Framework.
*    For each project:
*      Properties > Application > Target Framework > .Net Framework 4.6
*/
using System;                    // provides types and functionality used in all .Net programs
using System.Reflection;         // defines Assembly type
using System.Runtime.Remoting;   // provides remote communication between AppDomains
using System.Threading;          // provided by project wizard but not used here
using System.Security.Policy;    // defines evidence needed for AppDomain construction
using System.Collections.Generic;


namespace TestHarness1
{
    public class Creator
    {
        //----< show all assemblies currently loaded in AppDomain >------

        public static void showAssemblies(AppDomain ad)
        {
            Assembly[] arrayOfAssems = ad.GetAssemblies();
            foreach (Assembly assem in arrayOfAssems)
                Console.Write("\n  {0}", assem);
        }
        //----< catch exceptions thrown in child domain >----------------
        /*
         *  bool useTryCatch is provided so you can see that unhandled
         *  exceptions in child domain will take down the whole process.
         */
       public static int execute(IHello hello, List<string> names,bool useTryCatch = true) //return 1 success.return 0 fail, return 2 other situation
        {
            if (hello == null)
            {
                Console.Write("\n  hello reference is null\n");
               
            }
            if (useTryCatch)
            {
                try
                {
                    if (hello.say(names) == 1)
                    {
                        return 1;
                    }
                    else
                        return 0;
                }
                catch (System.Threading.ThreadAbortException ex)  // use more explicit catch conditions first
                {
                    Console.Write("\n  caught ThreadAbortException in main AppDomain");
                    Thread.ResetAbort();  // if you don't reset abort will be rethrown at end of catch clause
                }
                catch (Exception ex)
                {
                    Console.Write("\n  Exception caught in main domain: {0}", ex.Message);
                }
            }
            else  // not using try - catch
            {
                if (hello.say(names) == 1)
                {
                    return 1;
                }
                else return 0;
            }
            Console.WriteLine();
            return 2;
        }
        //----< main creates and demonstrates a child AppDomain >--------

        [LoaderOptimizationAttribute(LoaderOptimization.MultiDomainHost)]
        [STAThread]
        static void Main(string[] args)
        {
            Console.Write("\n  Application Domain Demo - Testing Error Handling");
            Console.Write("\n ==================================================");

            AppDomain ad = null;
            try
            {
                AppDomain main = AppDomain.CurrentDomain;
                Console.Write("\n  Starting in AppDomain {0}\n", main.FriendlyName);

                // Create application domain setup information for new AppDomain
                AppDomainSetup domaininfo = new AppDomainSetup();
                domaininfo.ApplicationBase
                  = "file:///" + System.Environment.CurrentDirectory;  // defines search path for assemblies

                //Create evidence for the new AppDomain from evidence of current
                Evidence adevidence = AppDomain.CurrentDomain.Evidence;

                // Create Child AppDomain
                ad = AppDomain.CreateDomain("ChildDomain", adevidence, domaininfo);

                /////////////////////////////////////////////////////////////////////
                //  Way to create ChildDomain using default evidence and domaininfo
                //    AppDomain ad = AppDomain.CreateDomain("ChildDomain", null);

                ad.Load("DemoClassLibrary");
                showAssemblies(ad);
                Console.Write("\n");

                // normal execution - same as first AppDomainDemo

                ObjectHandle oh
                  = ad.CreateInstance("ClassLibrary", "TestHarness1.hello1");
                TestHarness1.IHello h1 = (TestHarness1.IHello)oh.Unwrap();
                //h1.say();
           //     execute(h1,names);

                // code in child domain throws and catches
/*
                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello2");
                TestHarness1.IHello h2 = (TestHarness1.IHello)oh.Unwrap();
                execute(h2);

                // code in child domain throws but does not catch

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello3");
                TestHarness1.IHello h3 = (TestHarness1.IHello)oh.Unwrap();
                execute(h3);

                // code in child domain divides by zero and catches

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello4");
                TestHarness1.IHello h4 = (TestHarness1.IHello)oh.Unwrap();
                execute(h4);

                // code in child domain divides by zero but does not catch

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello5");
                TestHarness1.IHello h5 = (TestHarness1.IHello)oh.Unwrap();
                execute(h5);

                // code in child domain aborts and catches

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello6");
                TestHarness1.IHello h6 = (TestHarness1.IHello)oh.Unwrap();
                execute(h6);

                // code in child domain aborts but does not catch

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello7");
                TestHarness1.IHello h7 = (TestHarness1.IHello)oh.Unwrap();
                execute(h7);

                // code in child domain uses null reference and catches

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello8");
                TestHarness1.IHello h8 = (TestHarness1.IHello)oh.Unwrap();
                execute(h8);

                // code in child domain uses null reference but does not catch

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello9");
                TestHarness1.IHello h9 = (TestHarness1.IHello)oh.Unwrap();
                execute(h9);

                // hello10 does not exist so CreateInstance throws

                oh = ad.CreateInstance("DemoClassLibrary", "AppDomainDemo.hello10");
                TestHarness1.IHello h10 = (TestHarness1.IHello)oh.Unwrap();
                execute(h10);

                Console.Write("\n  All is well!  We got to end, so shutting down normally\n\n");*/
            }
            catch (Exception except)
            {
                Console.Write("\n  exception caught outside execute function:\n  {0}\n", except.Message);
                Console.Write("\n  Ending prematurely, but shutting down normally\n\n");
            }
            // unloading ChildDomain, and so unloading the library
           
            AppDomain.Unload(ad);
        }
    }
}
