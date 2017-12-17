using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Interface;
using DuPont.Repository;
using DuPont.Models.Models;
using EntityFramework.Extensions;

namespace DuPont.Repository
{
    public class NotificationRepository : BaseRepository<T_NOTIFICATION>, INotification
    {
        public bool ExistsPublicNotification()
        {
            string sql = @"SELECT   COUNT(1) AS Expr1
FROM      T_NOTIFICATION
WHERE   (IsPublic = 1) AND (IsDeleted = 0) AND (IsOnDate = 0) AND DATEDIFF(DD,CreateTime,GETDATE())=0 AND (MsgId NOT IN
                    (SELECT   MsgId
                     FROM      T_SEND_COMMON_NOTIFICATION_PROGRESS
                     WHERE   (DATEDIFF(DD, CreateTaskTime, GETDATE()) = 0) AND (LastMaxUserId >= (select top 1 Id from T_USER order by id desc))))";

            return SqlQuery<int>(sql).First() > 0;
        }


        public T_NOTIFICATION GetOneValidPublicNotification()
        {
            string sql = @"SELECT   *
FROM      T_NOTIFICATION
WHERE   (IsPublic = 1) AND (IsDeleted = 0) AND (IsOnDate = 0) AND DATEDIFF(DD,CreateTime,GETDATE())=0 AND (MsgId NOT IN
                    (SELECT   MsgId
                     FROM      T_SEND_COMMON_NOTIFICATION_PROGRESS
                     WHERE   (DATEDIFF(DD, CreateTaskTime, GETDATE()) = 0) AND (LastMaxUserId >= (select top 1 Id from T_USER order by id desc))))
order by MsgId asc";

            return SqlQuery<T_NOTIFICATION>(sql).FirstOrDefault();
        }


        public bool RemoveSameDeviceToken(string deviceToken)
        {
            using (var ctx = GetDbContextInstance())
            {
                return ctx.Set<T_USER>().Where(u => u.IosDeviceToken == deviceToken).Update(m => new T_USER { IosDeviceToken = null }) > 0;
            }
        }
    }
}
