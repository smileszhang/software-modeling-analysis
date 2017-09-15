using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Policy;    // defines evidence needed for AppDomain construction
using System.Runtime.Remoting;   // provides remote communication between AppDomains



namespace TestHarness1

{
    public class Client
    {
        [LoaderOptimizationAttribute(LoaderOptimization.MultiDomainHost)]
        [STAThread]

        static void Main(string[] args)
        {

            /*  // client chooses to got logs or do test
              string pathxml = "";

              Console.WriteLine("Plese choose:1.Reading logs or 2.taking test");
              int choose1 = int.Parse(Console.ReadLine());
              if (choose1 == 1)           //client chooses to read logs
              {
                  String pathlogs = "../../../Logs";
                  string[] logfiles = System.IO.Directory.GetFiles(pathlogs, "*.txt");
                  for (int i = 0; i < logfiles.Length; i++)
                  {
                      Console.WriteLine(i.ToString() + logfiles[i]);  //print logs in "Logs" file
                  }


                  int choose2 = int.Parse(Console.ReadLine());
                  if (logfiles[choose2] != null)
                  {
                      Logging.getlog(logfiles[choose2]);          //client gets log
                      return;
                  }
                  else
                  {             
                      return;
                  }
              }
              else if (choose1 == 2)   //client chooses to do test
              {
                  Console.WriteLine("Please choose test requests:" + "\n");
                  string path1 = "../../../XML test request";
                  string[] xmlfiles = System.IO.Directory.GetFiles(path1, "*.xml");
                  //List<int> counting = new List<int>();
                  //int i = 0;
                  for (int i = 0; i < xmlfiles.Length; i++)
                  {
                      Console.WriteLine(i.ToString() + xmlfiles[i]); //print xml files for client to choose
                  }


                  int choose3 = int.Parse(Console.ReadLine());
                  if (xmlfiles[choose3] != null)
                  {
                      pathxml = xmlfiles[choose3];   //set xml path
                  }
                  else return;

              }
              else return;*/
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("There are two test requests need to be parsed");
            Console.WriteLine("The first test request has two tests.");
            Console.WriteLine("The second test request has one test.");
       
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("\n");
            string pathxml = "../../../XML test request";
            string[] xmlfiles = System.IO.Directory.GetFiles(pathxml, "*.xml");
            for (int i = 0; i < xmlfiles.Length; i++)
            {
                XmlTest demo = new XmlTest();
                AppDomain ad = null;
                try
                {

                    /////////////////////////////////////////////////////////////
                    ///               Parse XML(Test Request) files           ///        
                    ////////////////////////////////////////////////////////////

                    // parse xml files from specific path
                    string path = xmlfiles[i];
                    System.IO.FileStream xml = new System.IO.FileStream(path, System.IO.FileMode.Open);
                    demo.parse(xml);
                    //xml.Close();
                    // List < Test > testlist= new List<Test>();
                    //testlist = demo.testList_;

                    /////////////////////////////////////////////////////////////
                    ///               create appdomain                        ///        
                    ////////////////////////////////////////////////////////////

                    AppDomain main = AppDomain.CurrentDomain;
                    Console.Write("\n  Starting in AppDomain {0}\n", main.FriendlyName);

                    // Create application domain setup information for new AppDomain
                    AppDomainSetup domaininfo = new AppDomainSetup();
                    domaininfo.ApplicationBase
                      = "file:///" + System.Environment.CurrentDirectory;  // defines search path for assemblies

                    //Create evidence for the new AppDomain from evidence of current
                    Evidence adevidence = AppDomain.CurrentDomain.Evidence;

                    // Create Child AppDomain to isolate each test
                    ad = AppDomain.CreateDomain("ChildDomain", adevidence, domaininfo);

                    ad.Load("ClassLibrary");
                    Creator.showAssemblies(ad);
                    Console.Write("\n");


                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine("Tests are tested in order.");
                    Console.WriteLine("-----------------------------------------");

                    foreach (Test test in demo.testList_)
                    {

                        //Queue
                        SWTools.BlockingQueue<string> q = new SWTools.BlockingQueue<string>();

                        // thread
                        Thread t = new Thread(() =>
                        {
                            string msg;
                            while (true)
                            {
                                msg = q.deQ();
                                Console.Write("\n  child thread received {0}", msg);  //child thread will handle each test
                                if (msg == "quit") break;

                                List<string> names = new List<string>();
                                names.Add(test.testDriver);

                                foreach (string library in test.testCode)
                                {
                                    names.Add(library);
                                }

                                //create logs
                                Logging logs = new Logging();

                                // give child appdomain instance
                                ObjectHandle oh
                                      = ad.CreateInstance("ClassLibrary", "TestHarness1.hello1");
                                TestHarness1.IHello h1 = (TestHarness1.IHello)oh.Unwrap();


                                int result = Creator.execute(h1, names);

                                if (result == 1)
                                {
                                    test.result = "test passed!!!!";
                                }
                                else
                                {
                                    test.result = "test failed!!!";
                                }
                                logs.writelog(test);

                                Console.WriteLine("\n" + test.result);
                            }
                        });
                        t.Start();

                        string temp = test.testName;
                        Console.Write("\n  main thread sending {0}", temp);
                        q.enQ(temp);
                        q.enQ("quit");  //end thread
                        t.Join();  
                        Console.Write("\n\n");

                    }
                }


                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}", ex.Message);
                }


                AppDomain.Unload(ad);
               



            }

            String pathlogs = "../../../Logs";
            string[] logfiles = System.IO.Directory.GetFiles(pathlogs, "*.txt");

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Getting logs by using getlog() function");
            Console.WriteLine("Logs are named by test");
            Console.WriteLine("-----------------------------------------");

            for (int i=0;i<logfiles.Length;i++)
            {
                Logging.getlog(logfiles[i]);
            }
            Console.ReadKey();
        }
        }
    }        
    

