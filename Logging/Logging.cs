using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace TestHarness1
{
    public class Logging
    {
        public static void getlog(string path)
        {
            Process p = new Process();
            p.StartInfo.FileName = path;
            p.Start();
        }
        public void writelog(Test test)
        {
           // Logging lg = new Logging();
            // System.IO.FileStream xml = new System.IO.FileStream(path1, System.IO.FileMode.Open); 
            //XmlTest demo = new XmlTest();
            //demo.parse(xml);
            //xml.Close();

            // FileStream fs = new FileStream("../../../Logs/" + xml.ToString() + ".txt", FileMode.OpenOrCreate, FileAccess.Write);


       
                StreamWriter sw = new StreamWriter("../../../Logs/" + test.testName + ".txt", true);
                //sw.Flush();
               // sw.BaseStream.Seek(0, SeekOrigin.Begin);
                
                    sw.WriteLine("Author:" + test.author);
                    sw.WriteLine("Testname:" + test.testName);
                    sw.WriteLine("TestDriver:" + test.testDriver);
                    sw.WriteLine("Test time:" + test.timeStamp);
                    sw.WriteLine("Test result:" + test.result);
                    sw.WriteLine("\n");

               // sw.Flush();
                sw.Close();
                //fs.Close();

               

            /*
             //string path = "../../../Logs/" + xml.ToString()+".txt";
            // FileStream fs = new FileStream(path, FileMode.Create);
            // StreamWriter sw = new StreamWriter(path,true);
            // for(int i=0;i<demo.testList_.Count;i++)
            {
                 sw.WriteLine("Author:" + test.author);
                 sw.WriteLine("Testname:" + test.testName);
                 sw.WriteLine("TestDriver:" + test.testDriver);
                 sw.WriteLine("Test time:" + test.timeStamp);
                 sw.WriteLine("Test result:" + test.result + "\n");
             }*/


            //清空缓冲区
            //sw.Flush();
            //关闭流
            // sw.Close();


        }
    }
}
