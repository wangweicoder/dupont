using DuPont.Utility;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace DuPont.ApiConsole
{
    class Program
    {
        public delegate string MethodCaller(string date);//定义个代理 
        public  void exic(ref string s)
        {
            s += "hello";
           // return s;
        }
        static void Main(string[] args)
        {
            string workdir = Directory.GetCurrentDirectory();//应用程序当前工作目录           
            if (1 == 1)
            {
                try
                {
                    //引用类型，是一个引用的地址，两个对象，指向同一个地址，修改了，会发生变化。
                    //student s1 = new student();
                    //student s2 = new student();
                    //s1.age = 18;Repository
                    //Utility,global,Repository,Presentation
                    //s2 = s1;
                    //s2.age = 20;
                    int o = 10;
                    int j = o;
                    j = 11;
                    Console.Write(string.Format("o:{0},j:{1}",o, j));

                    //Console.Write(string.Format("s1:{0},s2:{1}",s1.age,s2.age));
                    Console.Write("请输入要测试的方法，1为异步，2为同步:\r\n");
                    string e = Console.ReadLine().ToString();                 
                    //日期路径
                    //var cornurl = @"D:\DuPontWeb\ApiPresentation\FileJson";
                    //DirectoryInfo theFolder = new DirectoryInfo(cornurl);
                    ////DirectoryInfo[] dirInfo = theFolder.GetDirectories();
                    ////遍历文件夹
                    ////foreach (DirectoryInfo NextFolder in dirInfo)
                    ////{                       
                    //  FileInfo[] fileInfo = theFolder.GetFiles();
                    //    foreach (FileInfo NextFile in fileInfo)  //遍历文件
                    //    {
                    //        string filename=NextFile.Name;
                    //        Console.WriteLine(filename.Substring(0,filename.IndexOf('.')));
                    //    }
                    ////}
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    if (e == "1")
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            //string dt = Convert.ToDateTime("2017年04月13日").ToString("yyyy-MM-dd");
                            MethodCaller mc = TransfeDate;
                            string name = "2017-04-14,2017-06-23";//输入参数 
                            //AsyncCallback asyncballback = t => Console.WriteLine("这里是AsyncCallback回调{0},当前线程id{1}", t.AsyncState, Thread.CurrentThread.ManagedThreadId);
                            //IAsyncResult result = mc.BeginInvoke(name, asyncballback, ansycback("w"));                          
                            IAsyncResult result = mc.BeginInvoke(name, null,null);          
                            //if(!result.IsCompleted)
                            //{                 
                                 Console.WriteLine("{0},当前线程id{1}", "程序正在执行...", Thread.CurrentThread.ManagedThreadId);
                            //}
                            //string myname = mc.EndInvoke(result);//用于接收返回值 ,會耗時
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            string myname = TransfeDate("2017-04-14,2017-06-23");
                            Console.WriteLine("{0},当前线程id{1}", "程序正在执行...", Thread.CurrentThread.ManagedThreadId);
                        }
                    }
                    //string myname = TransfeDate("2017-04-14,2017-06-23,2017-06-23");
                    //string date = TransfeDate("2017年04月13日、2017年04月14日、");
                    sw.Stop();
                    TimeSpan ProGramts = sw.Elapsed;
                    Console.Write("运行时间：" + ProGramts.Seconds + "秒" + ProGramts.Milliseconds + "\r\n");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + ex.Message;
                    IOHelper.WriteLogToFile(logErrstring, workdir + @"\DuPontServiceyumiLog");
                }
            }

            Console.ReadLine();
        }
        private static string TransfeDate(string ExpectedDate)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("-{0},当前线程id{1}", ExpectedDate, Thread.CurrentThread.ManagedThreadId);              
            string resultdate = null;
            //干活日期 
            if (!string.IsNullOrWhiteSpace(ExpectedDate))
            {
                Thread.Sleep(100);
                for (int i = 0; i < 9999; i++)
                {
                    string tempdate = ExpectedDate;
                    if (tempdate.Contains(',') || tempdate.Contains('-'))
                    {
                        string[] tempdates = tempdate.Split(',');
                        foreach (var item in tempdates)
                        {
                            string idate = Convert.ToDateTime(item).ToString("yyyy年MM月dd日");
                            resultdate += idate + "、";
                        }
                        resultdate = resultdate.TrimEnd('、');
                    }
                    else if (tempdate.Contains("、"))
                    {
                        if (tempdate.EndsWith("、"))
                        {
                            tempdate = tempdate.TrimEnd('、');
                        }
                        resultdate = tempdate;
                    }
                }
                sw.Stop();
                TimeSpan ProGramts = sw.Elapsed;
                Console.WriteLine("-{0},当前线程id{1},耗时：{2}", ExpectedDate, Thread.CurrentThread.ManagedThreadId, +ProGramts.Seconds + "秒" + ProGramts.Milliseconds + "\r\n");
                return resultdate;
            }
            else
            {
                return ExpectedDate;
            }
        }
        public static string PostWithStandard(string url, IDictionary<string, string> parameters, string certificatePath, string certificatePassword)
        {
            var client = new RestSharp.RestClient(url);
            var request = new RestSharp.RestRequest(Method.POST);

            if (parameters != null)
            {
                foreach (var para in parameters)
                {
                    request.AddParameter(para.Key, para.Value);
                }
            }
            var response = client.Execute(request);


            return response.Content;

        }
        private  string ansycback(string o)
        {        
            return o;
        }
        /// <summary>
        /// 引用类型
        /// </summary>
        class student
        {
          public int age;
        }
        struct structstudent
        {
            public int age;
        }

    }
}
