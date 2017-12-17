
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.ApiConsole
{
    class Program1
    {
        static void Mainq(string[] args)
        {
            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 5000;//60000;//60秒
            //timer.Elapsed += timer_Elapsed;
            //timer.AutoReset = true;//true一直执行，false执行一次;
            //timer.Enabled = true;//是否执行Elapsed事件   

            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (1 == 1)
            {
                try
                {
                    //证书的路径
                    var certification = @"E:\01\_Daily_Commit\dotNet\DuPont\DuPont.Presentation\\App_Data\persentationapi.pfx";
                    //证书的密码
                    var certificationPwd = ConfigHelper.GetAppSetting("CertificatePwd");
                    string PresentationApiurl = ConfigHelper.GetAppSetting("RemotePresentationApi");
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("pageindex", "1");
                    dic.Add("pagesize", "10");
                    //string cornlist = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Common/GetModList", dic, certification, certificationPwd);
                   // var cresults = JsonHelper.FromJsonTo<ResponseResult<object>>(cornlist);
                   // if (cresults.IsSuccess == true)
                   // {
                    //    object o = cresults.Entity;
                   // }
                    //long rows = 0;
                    //string s = "116.405285,39.90498911496372238826617Max • ♪动力机械 雷沃 1234,轮式拖拉机 雷沃 jjdjd,动力机械 雷沃 123,动力机械 雷沃 gjgdybc186129707791A687783A42B043ECB9C42496CA8858EA44945";
                    // var keyString = TransferEncoding(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, s);
                    ////var encryptedAuthorizedStr = new Encrypt().SHA256_Encrypt(s);
                    // //byte[] srcBytes = Encoding.UTF8.GetBytes(s);
                    // //var keyString = Encoding.UTF8.GetString(srcBytes);
                    // string ecode = "%3C%2Fsscriptcript%09%3E %3Csscriptcript%3E%5Bwindow%5B%22location%22%5D%3D%22%5Cx6a%5Cx61%5Cx76%5Cx61%5Cx73%5Cx63%5Cx72%5 Cx69%5Cx70%5Cx74%5Cx3a%5Cx61%5Cx6c%5Cx65%5Cx72%5Cx74%5Cx28297%5Cx29%22%5D%3C%2Fsscriptcript%3E";
                    // var ds = System.Web.HttpUtility.UrlDecode(ecode);
                    // var ds2 = System.Web.HttpUtility.UrlEncode("c# url解码");
                    Dictionary<string, string> content = new Dictionary<string, string>();
                    //string orderlist = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Account/UpdateBusinessRequirementState", content, null, null);
                    //var results = JsonHelper.FromJsonTo<ResponseResult<object>>(orderlist);
                    //if (results.IsSuccess == true)
                    //{
                    //    rows = results.TotalNums;
                    //}


                    string modlist = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Common/GetModPriceUrl", content, certification, certificationPwd);
                    var everydayurllist = JsonHelper.FromJsonTo<ResponseResult<CornDayUrlModel>>(modlist);
                    //foreach (var item in everydayurllist.Entity.dayurllist)
                    //{
                    Dictionary<string, string> urlcontent = new Dictionary<string, string>();
                    urlcontent.Add("everydayurl", everydayurllist.Entity.dayurllist[0].href);
                    string result = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Common/GetCornPrices", urlcontent, certification, certificationPwd);

                    //}
                    sw.Stop();
                    TimeSpan ProGramts = sw.Elapsed;
                    //Console.Write(ds);
                    Console.Write("运行时间：" + ProGramts.Seconds + "秒" + ProGramts.Milliseconds+"毫秒");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + ex.Message;
                    IOHelper.WriteLogToFile(logErrstring, @"D:\DuPontServiceLog\DuPontServiceyumi.txt");
                }
            }

            Console.ReadLine();
        }
        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="srcEncoding">原编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <param name="srcBytes">原字符串</param>
        /// <returns>字符串</returns>
        public static string TransferEncoding(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            byte[] bytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
            return dstEncoding.GetString(bytes);

        }
        //static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    int intHour = e.SignalTime.Hour;
        //    int intMinute = e.SignalTime.Minute;
        //    int intSecond = e.SignalTime.Second;
        //    //if (intHour == 11 && intMinute == 39)
        //    if (1 == 1)
        //    {
        //        try
        //        {
        //            //需求列表接口
        //            IPublished_Demand _DemandService = new DuPont.Repository.Published_DemandRepository();
        //            //修改大农户需求
        //            IFarmerRequirement _farmerRequirement = new DuPont.Repository.FarmerRequirementRepository();
        //            List<FarmerDemand> farmerlist = new List<FarmerDemand>();
        //            long TotalNums = 0;
        //            farmerlist = _DemandService.GetFarmerDemandList(1, 9999, 0, out TotalNums);
        //            DateTime nowtime = DateTime.Now;
        //            DateTime dtThis = new DateTime(Convert.ToInt32(nowtime.Year), Convert.ToInt32(nowtime.Month), Convert.ToInt32(nowtime.Day));
        //            for (int i = 0; i < farmerlist.Count; i++)
        //            {
        //                DateTime ReceiveDate = farmerlist[i].ReceiveDate;
        //                DateTime dtLast = new DateTime(Convert.ToInt32(ReceiveDate.Year), Convert.ToInt32(ReceiveDate.Month), Convert.ToInt32(ReceiveDate.Day));
        //                int interval = new TimeSpan(dtThis.Ticks - dtLast.Ticks).Days;
        //                if (interval > 30)
        //                {
        //                    var entityId = Convert.ToInt64(farmerlist[i].DemandId);
        //                    var entity = _farmerRequirement.GetByKey(entityId);
        //                    if (entity.PublishStateId == 100505)//系统关闭
        //                    {
        //                        break;
        //                    }
        //                    entity.PublishStateId = 100505;
        //                    _farmerRequirement.Update(entity);
        //                }
        //            }
        //            sw.Stop();
        //            TimeSpan ProGramts = sw.Elapsed;
        //            Console.Write("运行时间：" + ProGramts.Seconds + "秒" + ProGramts.Milliseconds);
        //            Console.ReadLine();
        //        }
        //        catch (Exception ex)
        //        {
        //            string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + ex.Message;
        //            IOHelper.WriteLogToFile(logErrstring, @"D:\DuPontServiceLog\DuPontService.txt");
        //        }
        //    }
        //}

    }
}
