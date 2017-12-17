using DuPont.Attributes;
using DuPont.Entity.Enum;
using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Models;
using DuPont.Models.Models;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace DuPont.Controllers
{
    [CustomHandleErrorWithErrorJson]
    public class LogsController : BaseController
    {
        private ILog LogRepository;
        private IUser userRepository;
        public LogsController(IPermissionProvider permissionManage,
            ILog LogRepository, IUser userRepository)
            : base(permissionManage)
        {
            this.LogRepository = LogRepository;
            this.userRepository = userRepository;
        }
        /// <summary>
        /// 根据角色获取 查看报表列表权限
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(long userId, int pageIndex = 1, int pageSize = 10)
        {
            var result = new ResponseResult<ListLogViewModel>();

            var roleList = userRepository.GetRoleList(userId);
            var bo = roleList.Any(model =>
                        {
                            return model.RoleId == (int)RoleType.SuperAdmin;
                        });
            long recordCount;
            if (bo)
            {
                var logListModel = LogRepository.GetLogListModel(pageIndex, pageSize, out recordCount);
                var listMode = new ListLogViewModel()
                {
                    Pager = new PagedList<string>(new string[0], pageIndex, pageSize, (int)recordCount),
                    listModel = logListModel
                };
                result.TotalNums = recordCount;
                result.PageIndex = pageIndex;
                result.PageSize = pageSize;
                result.Entity = listMode;
                result.IsSuccess = true;
                result.Message = "返回成功";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "无访问权限!";
            }

            return new JsonResultEx(result);
        }
    }
}