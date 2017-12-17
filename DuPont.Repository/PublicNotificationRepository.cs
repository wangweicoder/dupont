using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Repository;
using DuPont.Models.Models;
using DuPont.Interface;
using System.Data.SqlClient;

namespace DuPont.Repository
{
    public class PublicNotificationRepository : BaseRepository<T_SEND_COMMON_NOTIFICATION_PROGRESS>, IPublicNotification
    {
        private readonly IPersonalNotification _personalNotificationService;
        private readonly IUser _userService;
        public PublicNotificationRepository(IPersonalNotification personalNotificationService,
            IUser userService
            )
        {
            _personalNotificationService = personalNotificationService;
            _userService = userService;
        }

        public List<T_USER> GetPublicNotificationValidUser(IEnumerable<T_USER> users, long msgId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT   UserId");
            var userCount = users.Count();
            for (int i = 0; i < userCount; i++)
            {
                var user = users.ElementAt(i);
                if (i == 0)
                {
                    sql.AppendLine("FROM      (SELECT   " + user.Id + " AS 'UserId'");
                }
                else if (i != userCount - 1)
                {
                    sql.AppendLine("                 UNION");
                    sql.AppendLine("                 SELECT   " + user.Id + " AS Expr1");
                }
                else
                {
                    sql.AppendLine("                 UNION");
                    sql.AppendLine("                 SELECT   " + user.Id + " AS Expr1) AS tt");
                }
            }

            sql.AppendLine("WHERE   UserId NOT IN");
            sql.AppendLine("                    (SELECT   UserId");
            sql.AppendLine("                     FROM      T_SEND_NOTIFICATION_RESULT");
            sql.AppendLine("                     WHERE   (MsgId = @MsgId))");


            var validUserIdList = _personalNotificationService.SqlQuery<int>(sql.ToString(), new SqlParameter("@MsgId", msgId));

            if (validUserIdList == null || !validUserIdList.Any())
                return null;

            var userIdIntArray = validUserIdList.ToArray();
            var userIdLongArray = new long[userIdIntArray.Length];
            Array.Copy(userIdIntArray, 0, userIdLongArray, 0, userIdLongArray.Length);

            var models = _userService.GetAll(m => userIdLongArray.Contains(m.Id)).ToList();
            return models;
        }
    }
}
