/////////////////////////////////////////////////////////////////////
// XmlTest.cs - Help Session Demonstration of XML Processing       //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, F2016     //
/////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness1
{
    public class Test
    {
        public string testName { get; set; }
        public string author { get; set; }
        public DateTime timeStamp { get; set; }
        public String testDriver { get; set; }
        public List<string> testCode { get; set; }
        public string result = "not test";
        public void show()
        {
            Console.Write("\n  {0,-12} : {1}", "test name", testName);
            Console.Write("\n  {0,12} : {1}", "author", author);
            Console.Write("\n  {0,12} : {1}", "time stamp", timeStamp);
            Console.Write("\n  {0,12} : {1}", "test driver", testDriver);
            


            foreach (string library in testCode)
            {
                Console.Write("\n  {0,12} : {1}", "library", library);
            }
            Console.Write("\n  {0,12}:{1}", "test result", result);
        }
    }
    public class XmlTest
    {
        public XDocument doc_;
        public List<Test> testList_;
        public XmlTest()
        {
            doc_ = new XDocument();
            testList_ = new List<Test>();
        }
        public bool parse(System.IO.FileStream xml)
        {
            doc_ = XDocument.Load(xml);
            if (doc_ == null)
                return false;
            string author = doc_.Descendants("author").First().Value;
            Test test = null;

            XElement[] xtests = doc_.Descendants("test").ToArray();
            int numTests = xtests.Count();

            for (int i = 0; i < numTests; ++i)
            {
                test = new Test();
                test.testCode = new List<string>();
                test.author = author;
                test.timeStamp = DateTime.Now;
                test.testName = xtests[i].Attribute("name").Value;
                test.testDriver = xtests[i].Element("testDriver").Value;
                IEnumerable<XElement> xtestCode = xtests[i].Elements("library");
                foreach (var xlibrary in xtestCode)
                {
                    test.testCode.Add(xlibrary.Value);
                }
                testList_.Add(test);
            }
            return true;
        }
        static void Main(string[] args)
        {
            XmlTest demo = new XmlTest();
            try
            {
                string path = "../../TestRequest.xml";
                System.IO.FileStream xml = new System.IO.FileStream(path, System.IO.FileMode.Open);
                demo.parse(xml);
                foreach (Test test in demo.testList_)
                {
                    test.show();
                }
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}", ex.Message);
            }
        }
    }
}
