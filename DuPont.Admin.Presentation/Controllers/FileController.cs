using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
    public class FileController : Controller
    {
        [HttpPost]
        public string Upload()
        {
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //上传配置
            string pathbase = "/upload/";                                                          //保存路径
            int size = 10;                     //文件大小限制,单位mb                                                                                   //文件大小限制，单位KB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };                    //文件允许格式

            string callback = Request["callback"];
            string editorId = Request["editorid"];

            //上传图片
            Hashtable info;
            Uploader up = new Uploader();
            info = up.upFile(this.HttpContext, pathbase, filetype, size); //获取上传状态
            string json = BuildJson(info);

            Response.ContentType = "text/html";
            if (callback != null)
            {
                return String.Format("<script>{0}(JSON.parse(\"{1}\"));</script>", callback, json);
            }
            else
            {
                return json;
            }
        }

        private string BuildJson(Hashtable info)
        {
            List<string> fields = new List<string>();
            string[] keys = new string[] { "originalName", "name", "url", "size", "state", "type" };
            for (int i = 0; i < keys.Length; i++)
            {
                fields.Add(String.Format("\"{0}\": \"{1}\"", keys[i], info[keys[i]]));
            }
            return "{" + String.Join(",", fields) + "}";
        }
	}
}