/////////////////////////////////////////////////////////////////////
// TestHarness.cs - Runs tests by loading dlls and invoking test() //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2016 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TestHarness1
{
    public class TestHarness
    {
        public List<string> LIST=new List<string>();
        public struct TestData
        {
            public string Name;
            public ITest testDriver;
        }

        public List<TestData> testDriver = new List<TestData>();

        public TestHarness() { }

        //----< load test dlls to invoke >-------------------------------

        public List<string> getTestDLL(List<string> names)
        {
            foreach (var name in names)
            {
                LIST.Add("../../../Tests/" + name);
            }
            return LIST;
        }

        //private ArrayList getDLLFiles(string path)
        //{
        //    string[] files = System.IO.Directory.GetFiles(path,"*.dll");
        //    ArrayList ar = new ArrayList();
        //    foreach (var file in files)
        //    {
        //        foreach (var re in LIST)
        //        {
        //            if (file==re)
        //            {
        //                ar.Add(re);
        //            }
        //        }
        //    }
        //    return ar;
            
        //}

       public bool LoadTests(List<string> names)
        {
            try
            {
              

               //string[] files = System.IO.Directory.GetFiles(path, "*.dll");           
//                Console.WriteLine("{0}", System.IO.Directory.GetCurrentDirectory());
                foreach(string file in names)
                {
                    Console.Write("\n  loading: \"{0}\"", file);

                    Assembly assem = Assembly.LoadFrom("../../../Tests/" +file);

                    Type[] types = assem.GetExportedTypes();

                    foreach (Type t in types)
                    {
                        if (t.IsClass && typeof(ITest).IsAssignableFrom(t))  // does this type derive from ITest ?
                        {
                            ITest tdr = (ITest)Activator.CreateInstance(t);    // create instance of test driver

                            // save type name and reference to created type on managed heap

                            TestData td = new TestData();
                            td.Name = t.Name;
                            td.testDriver = tdr;
                            testDriver.Add(td);
                        }
                    }
                }
                Console.Write("\n");
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}\n\n", ex.Message);
                return false;
            }
            return testDriver.Count > 0;   // if we have items in list then Load succeeded
        }
        //----< run all the tests on list made in LoadTests >------------

       public int run()  //return 1 success.return 0 fail.return 2 other situation
        {
            if (testDriver.Count == 0)
                return 2;
            foreach (TestData td in testDriver)  // enumerate the test list
            {
                Console.Write("\n  testing {0}", td.Name);
                if (td.testDriver.test() == true)
                {
                    //Console.Write("\n  test passed");
                    return 1;
                }
                else
                {
                   // Console.Write("\n  test failed");
                    return 0;
                }
            }
            return 2;        
        }

        static void Main(string[] args)
        {
            // using string path = "../../../Tests/TestDriver.dll" from command line;

            if (args.Count() == 0)
            {
                Console.Write("\n  Please enter path to libraries on command line\n\n");
                return;
            }
            string path = args[0];
            List<string> s = new List<string>();
            TestHarness th = new TestHarness();
            if (th.LoadTests(s))
                th.run();
            else
                Console.Write("\n  couldn't load tests");

            Console.Write("\n\n");
        }
    }
}
