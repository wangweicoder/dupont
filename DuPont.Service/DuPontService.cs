using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;


namespace DuPont.Service
{

    public partial class DuPontService : ServiceBase
    {
        //日志路径
        string workUrl = ConfigHelper.GetAppSetting("LogPath");
        //接口地址
        string PresentationApiurl = ConfigHelper.GetAppSetting("RemotePresentationApi");
        public DuPontService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("DuPont定时更新订单已完成服务");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;//60秒
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;//true一直执行，false执行一次;
            timer.Enabled = true;//是否执行Elapsed事件
            System.Timers.Timer timeryumi = new System.Timers.Timer();
            timeryumi.Interval = 60000 * 10;//10分钟
            timeryumi.Elapsed += timeryumi_Elapsed;
            timeryumi.AutoReset = true;//true一直执行，false执行一次;
            timeryumi.Enabled = true;//是否执行Elapsed事件
        }
        /// <summary>
        /// 获取证书密码
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GetCertificationPwd()
        {
            return ConfigHelper.GetAppSetting("CertificatePwd");
        }

        /// <summary>
        /// 获取证书地址
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GetCertificationFilePath()
        {
            return ConfigHelper.GetAppSetting("CertificateUrl");
        }
        /// <summary>
        /// 保存玉米价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timeryumi_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int intHour = e.SignalTime.Hour;
            if (intHour >= 9 && intHour <= 12)
            {
                try
                {
                    IOHelper.WriteLogToFile("yumiprogram start \r\n", workUrl + @"\DuPontServiceyumiLog");

                    Dictionary<string, string> content = new Dictionary<string, string>();
                    string modlist = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Common/GetModPriceUrl", content, GetCertificationFilePath(), GetCertificationPwd());
                    var everydayurllist = JsonHelper.FromJsonTo<ResponseResult<CornDayUrlModel>>(modlist);
                    if (everydayurllist.Entity != null)
                    {
                        Dictionary<string, string> urlcontent = new Dictionary<string, string>();
                        urlcontent.Add("everydayurl", everydayurllist.Entity.dayurllist[0].href);
                        string result = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Common/GetCornPrices", urlcontent, GetCertificationFilePath(), GetCertificationPwd());
                    }
                    sw.Stop();
                    TimeSpan ProGramts = sw.Elapsed;
                    string Protimespan = ProGramts.Seconds + "秒" + ProGramts.Milliseconds + "毫秒";
                    IOHelper.WriteLogToFile("运行" + Protimespan + "\r\n", workUrl + @"\DuPontServiceyumiLog");
                }
                catch (Exception ex)
                {
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + ex.Message;
                    IOHelper.WriteLogToFile(logErrstring, workUrl + @"\DuPontServiceyumiLog");
                }
            }
        }
        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;
            if (intHour == 23 && intMinute == 50)
            {
                long rows = 0;
                try
                {
                    IOHelper.WriteLogToFile("program start \r\n", workUrl + @"\DuPontServiceLog");
                    Dictionary<string, string> content = new Dictionary<string, string>();
                    string orderlist = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Account/UpdateFarmerRequirementState", content, GetCertificationFilePath(), GetCertificationPwd());
                    var result = JsonHelper.FromJsonTo<ResponseResult<object>>(orderlist);
                    if (result.IsSuccess == true)
                    {
                        rows = result.TotalNums;
                    }
                    sw.Stop();
                    TimeSpan ProGramts = sw.Elapsed;
                    string Protimespan = ProGramts.Seconds + "秒" + ProGramts.Milliseconds + "毫秒";
                    string logstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "修改大农户需求" + rows + "条";
                    IOHelper.WriteLogToFile(logstring + "运行" + Protimespan + "\r\n", workUrl + @"\DuPontServiceLog");
                    //更新产业商订单
                    string borderlist = HttpAsynchronousTool.CustomHttpWebRequestPost(PresentationApiurl + "/Account/UpdateBusinessRequirementState", content, GetCertificationFilePath(), GetCertificationPwd());
                    var bresult = JsonHelper.FromJsonTo<ResponseResult<object>>(orderlist);
                    if (bresult.IsSuccess == true)
                    {
                        rows = bresult.TotalNums;
                    }
                    sw.Stop();
                    TimeSpan bProGramts = sw.Elapsed;
                    string bProtimespan = ProGramts.Seconds + "秒" + ProGramts.Milliseconds + "毫秒";
                    string blogstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "修改产业商需求" + rows + "条";
                    IOHelper.WriteLogToFile(blogstring + "运行" + Protimespan + "\r\n", workUrl + @"\DuPontServiceLog");

                }
                catch (Exception ex)
                {
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + ex.Message;
                    IOHelper.WriteLogToFile(logErrstring, workUrl + @"\DuPontServiceLog");
                }
            }
        }

        protected override void OnStop()
        {

            IOHelper.WriteLogToFile("Service Stop", workUrl + @"\DuPontServiceLog");

        }
    }
}
