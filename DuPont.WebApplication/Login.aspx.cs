using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DuPont.WebApplication
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var s = Session["user"];
            }
            else
            {
                var s = Session["user"];
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var LoginUserName = TextBox1.Text;
            var Password = TextBox2.Text;
            if (LoginUserName == "DuPontTest" && Password.Contains("dupont123"))
            {
               // System.Web.HttpContext.Current.Application["num"] = 1;
              Session["user"] = LoginUserName;
              
               Response.Redirect("DuPontApiTest.aspx");
               //handersession();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        private void handersession()
        {
            if (!IsPostBack)
            {
                var s = Session["user"];
            }
            else
            {
                var s = Session["user"];
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var s = Session["user"];
            }
            else
            {
                var s = Session["user"];
            }
        }
    }
}