using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Extensions;

namespace DuPont.Utility
{
    public static class ModelHelper
    {
        public static Dictionary<string, string> GetPropertyDictionary<T>(T t_class) where T : new()
        {
            if (t_class == null)
                throw new ArgumentNullException("t_class");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type t = t_class.GetType();
            //获取所有公共属性
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                //判断当前属性是否是值类型
                if (property.PropertyType.IsValueType)
                {
                    var value = property.GetValue(t_class, null);

                    //属性名为key,属性值为value存入字典中
                    dic[property.Name] = value != null ? value.ToString() : Convert.ToString(property.PropertyType.GetDefault());
                }
                else
                {
                    dic[property.Name] = property.GetValue(t_class, null) != null ? property.GetValue(t_class, null).ToString() : null;
                }
            }
            return dic;
        }

        #region "Type类型扩展"
        public static object GetDefault(this Type type)
        {
            return typeof(ModelHelper).GetMethod("GetDefaultImp").MakeGenericMethod(type).Invoke(null, new Type[0]);
        }
        public static T GetDefaultImp<T>()
        {
            return default(T);
        }
        #endregion
    }
}
