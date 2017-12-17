using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DuPont.Extensions
{
    public static class Extensions
    {
        #region "DataSet的扩展"
        /// <summary>           
        /// DataSet转换为泛型集合           
        /// </summary>           
        /// <typeparam name="T">泛型类型</typeparam>           
        /// <param name="ds">DataSet数据集</param>           
        /// <param name="tableIndex">待转换数据表索引,默认第0张表</param>           
        /// <returns>返回泛型集合</returns>           
        public static IList<T> ToList<T>(this DataSet ds, int tableIndex = 0)
        {
            if (ds == null || ds.Tables.Count < 0) return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;
            DataTable dt = ds.Tables[tableIndex];
            // 返回值初始化               
            IList<T> result = new List<T>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值                           
                        if (pi.Name.Equals(dt.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理                               
                            if (dt.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, dt.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        /// <summary>           
        /// DataSet转换为泛型集合           
        /// </summary>           
        /// <typeparam name="T">泛型类型</typeparam>           
        /// <param name="ds">DataSet数据集</param>           
        /// <param name="tableName">待转换数据表名称,名称为空时默认第0张表</param>           
        /// <returns>返回泛型集合</returns>           
        public static IList<T> ToList<T>(this DataSet ds, string tableName)
        {
            int _TableIndex = 0;
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (string.IsNullOrEmpty(tableName))
                return ToList<T>(ds, 0);
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                // 获取Table名称在Tables集合中的索引值                   
                if (ds.Tables[i].TableName.Equals(tableName))
                {
                    _TableIndex = i;
                    break;
                }
            }
            return ToList<T>(ds, _TableIndex);
        }
        #endregion

        #region "DataTable的扩展"
        /// <summary>           
        /// DataTable转换为泛型集合           
        /// </summary>           
        /// <typeparam name="T">泛型类型</typeparam>           
        /// <param name="dt">DataTable数据表</param>           
        /// <returns>返回泛型集合</returns>           
        public static IList<T> ToList<T>(this DataTable dt) where T : class,new()
        {
            if (dt == null || dt.Rows.Count <= 0) return null;
            List<T> list = new List<T>();
            #region 方法一：
            //T model;  
            //Type infos = typeof(T);  
            ////object tempValue;  
            //foreach (DataRow dr in dt.Rows)  
            //{  
            //    model = new T();  
            //    1.  
            //    infos.GetProperties().ToList().ForEach(p =>p.SetValue(model, dr[p.Name], null));  
            //     2.  
            //    //infos.GetProperties().ToList().ForEach(p =>  
            //    //{  
            //    //    tempValue = dr[p.Name];  
            //    //    if (!string.IsNullOrEmpty(tempValue.ToString()))  
            //    //        p.SetValue(model, tempValue, null);  
            //    //});  
            //    list.Add(model);  
            //}  
            #endregion
            #region 方法二：    比方法一快
            PropertyInfo[] propertys = typeof(T).GetProperties();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo pi in propertys)
                {
                    // 属性与字段名称一致的进行赋值                           
                    if (pi.Name.Equals(dt.Columns[pi.Name].ColumnName))
                    {
                        if (dt.Rows[j][pi.Name] != DBNull.Value)
                            pi.SetValue(_t, dt.Rows[j][pi.Name], null);
                        else
                            pi.SetValue(_t, null, null);
                    }
                }
                list.Add(_t);
            }
            #endregion
            return list;
        }
        #endregion

        #region "Enum类的扩展"
        /// <summary>
        /// 获取枚举的Description注解属性值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }
        #endregion

        #region "String类的扩展"
        /// <summary>
        /// 判断对象是否为NULL
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 检查字符串是否为NULL或空值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 检查字符串是否为Url地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(this string url)
        {
            //string pattern = @"/^http[s]?://"+
            //                 "(([0-9]{1,3}.){3}[0-9]{1,3}"+             // IP形式的URL- 199.194.52.184 
            //                 "|"+                                       // 允许IP和DOMAIN（域名） 
            //                 "([0-9a-z_!~*'()-]+.)*"+                   // 三级域验证- www. 
            //                 "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]."+     // 二级域验证 
            //                 "[a-z]{2,6})"+                             // 顶级域验证.com or .museum 
            //                 "(:[0-9]{1,4})?"+                          // 端口- :80 
            //                 "((/?)|"+                                  // 如果含有文件对文件部分进行校验 
            //                 "(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-/]*)?)$/";

            //string pattern = @"/^http[s]?://(([0-9]{1,3}.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z].[a-z]{2,6})(:[0-9]{1,4})?((/?)|(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-/]*)?)$/";
            string pattern = @"/^(http://|https://)?((?:[A-Za-z0-9]+-[A-Za-z0-9]+|[A-Za-z0-9]+)\.)+([A-Za-z]+)[/\?\:]?.*$/";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(url);
        }

        /// <summary>
        /// 将指定的默认字符串替值为空的原始字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string DefaultIfEmpty(this string source, string defaultValue)
        {
            return string.IsNullOrEmpty(source) ? defaultValue : source;
        }
        #endregion

        #region "List类的扩展"
        /// <summary>
        /// 判断集合中是否有空元素
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool HasNullElement(this  List<Object> obj)
        {
            return obj.Any(item => obj == null);
        }
        #endregion

        #region "HttpResponseBase类的扩展"
        /// <summary>
        /// 输出带有错误样式的消息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public static void WriteErrorMessage(this HttpResponseBase response, string message)
        {
            response.Write(string.Format("<span style=\"color:red\">{0}</span>", HttpUtility.HtmlEncode(message)));
        }
        #endregion

        #region "HttpRequest类的扩展"

        private static string GetRequestString(string httpMethod, HttpRequestBase request)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("-----------------------------------------");
            if (httpMethod == "GET")
            {
                //记录请求头信息
                stringBuilder.AppendLine("*****Request Header*****");
                foreach (var paramKey in request.Headers.AllKeys)
                {
                    stringBuilder.AppendFormat("{0}:{1}\r\n", paramKey, request.Headers[paramKey] ?? "");
                }

                stringBuilder.AppendLine("-----------------------------------------");

                //记录请求体信息
                stringBuilder.AppendLine("*****Request Body*****");
                foreach (var paramKey in request.QueryString.AllKeys)
                {
                    stringBuilder.AppendFormat("{0}:{1}\r\n", paramKey, request.QueryString[paramKey] ?? "");
                }
                stringBuilder.AppendLine("-----------------------------------------");
                stringBuilder.AppendLine();
            }
            else if (httpMethod == "POST")
            {
                //记录请求头信息
                stringBuilder.AppendLine("*****Request Header*****");
                foreach (var paramKey in request.Headers.AllKeys)
                {
                    stringBuilder.AppendFormat("{0}:{1}\r\n", paramKey, request.Headers[paramKey] ?? "");
                }

                stringBuilder.AppendLine("-----------------------------------------");

                //记录请求体信息
                stringBuilder.AppendLine("*****Request Body*****");
                foreach (var paramKey in request.Form.AllKeys)
                {
                    stringBuilder.AppendFormat("{0}:{1}\r\n", paramKey, request.Form[paramKey] ?? "");
                }
                stringBuilder.AppendLine("-----------------------------------------");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取当前GET请求的相关参数信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestBodyString(this HttpRequestBase request)
        {
            return GetRequestString(request.HttpMethod.ToUpper(), request);
        }
        #endregion

        #region "DbContext扩展"
        public static void Update<TEntity>(this DbContext dbContext, Expression<Func<TEntity, object>> propertyExpression, params TEntity[] entities)
        where TEntity : EntityBase
        {
            if (propertyExpression == null) throw new ArgumentNullException("propertyExpression");
            if (entities == null) throw new ArgumentNullException("entities");
            ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;
            foreach (TEntity entity in entities)
            {
                try
                {
                    DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
                    entry.State = EntityState.Unchanged;
                    foreach (var memberInfo in memberInfos)
                    {
                        entry.Property(memberInfo.Name).IsModified = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity originalEntity = dbContext.Set<TEntity>().Local.Single(m => m.Id == entity.Id);
                    ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
                    ObjectStateEntry objectEntry = objectContext.ObjectStateManager.GetObjectStateEntry(originalEntity);
                    objectEntry.ApplyCurrentValues(entity);
                    objectEntry.ChangeState(EntityState.Unchanged);
                    foreach (var memberInfo in memberInfos)
                    {
                        objectEntry.SetModifiedProperty(memberInfo.Name);
                    }
                }
            }
        }
        #endregion



    }
}
