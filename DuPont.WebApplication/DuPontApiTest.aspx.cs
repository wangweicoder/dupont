using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DuPont.WebApplication
{
    public partial class DuPontApiTest : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {            
            //HttpContext.Current.Request.Cookies["Login"] 
            if (Session["user"] != null || System.Web.HttpContext.Current.Application["num"] != null)
           {                       
                Addvalue();
                if (!IsPostBack)
                {
                    int sum = 1;
                    lbltitle1.Text = "测试接口" + sum++ + lbltitle1.Text;
                    lbltitle2.Text = "测试接口" + sum + lbltitle2.Text;
                    lbltitle3.Text = "测试接口" + ++sum + lbltitle3.Text;
                    lbltitle4.Text = "测试接口" + ++sum + lbltitle4.Text;
                    lbltitle5.Text = "测试接口" + ++sum +lbltitle5.Text;
                    lbltitle6.Text = "测试接口" + ++sum + lbltitle6.Text;
                }
                else
                {
                    int sum = (int)System.Web.HttpContext.Current.Application["num"];
                    lbltitle1.Text = "测试接口" + sum++ + lbltitle1.Text;
                    lbltitle2.Text = "测试接口" + sum + lbltitle2.Text;
                    lbltitle3.Text = "测试接口" + ++sum + lbltitle3.Text;
                    lbltitle4.Text = "测试接口" + ++sum + lbltitle4.Text;
                    lbltitle5.Text = "测试接口" + ++sum + lbltitle5.Text;
                    lbltitle6.Text = "测试接口" + ++sum + lbltitle6.Text;
                }

            }
           else
           {
               Response.Redirect("Login.html");
           }
        }

        private void Addvalue()
        {
            //发布需求单
            txtarea1.Value = @"userid:58;
id:0;
type:100103;
cropId:100302;
acreage:100901;
description:6月5号;
date:2017年06月2日、2017年06月3日;
address:110000000000|110100000000|110101000000|110101001000|110101001001;
ExpectedStartPrice:2;
ExpectedEndPrice:4;
PhoneNumber:13161203896";
            //先锋邦农机手评价
            txtarea2.Value = @"id:251;
OperatorUserid:57;
FarmerUserId:58;
CommentString:评价大农户;
Score:4;
SourceType:0";
            //E田接单
            txtarea3.Value = @"id:617
userId:44945;
Name:贺祥;
NickName:Max • ♪;Address:116.405285,39.904989;PhoneNumber:18612970779;
OtherMachinery:动力机械 雷沃 1234,轮式拖拉机 雷沃 jjdjd,动力机械 雷沃 123,动力机械 雷沃 gjgdybc;
Credit:1";
            //4、先锋帮大农户评价靠谱作业的农机手
            txtarea4.Value = @"id:613;
executeUserId:58;
userid:145;
commentString:评价靠谱作业农机手;
score:5;
SourceType:1";
            //5、靠谱作业的农机手评价先锋帮大农户
            txtarea5.Value = @"id:65;
OperatorUserid:72;
FarmerUserId:23;
CommentString:评价;
Score:5;
SourceType:1";
            //6、靠谱作业的农机手评价先锋帮大农户
            txtarea6.Value = @"id:669;
OperatorUserId:203;
FarmerUserId:58;
OrderState:539";
        }
    }
}