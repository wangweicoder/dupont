// ***********************************************************************
// Assembly         : DuPont.Utility
// Author           : 毛文君
// Created          : 08-27-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-27-2015
// ***********************************************************************
// <copyright file="JsonHelper.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DuPont.Utility
{
    public class JsonHelper
    {
        //json 序列化
        public static string ToJsJson(object item)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            }
        }

        //反序列化
        public static T FromJsonTo<T>(string jsonString)
        {
            if (jsonString==null)
            {
                return default(T);
            }
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T jsonObject = (T)ser.ReadObject(ms);
                return jsonObject;
            }
        }
    }
}
