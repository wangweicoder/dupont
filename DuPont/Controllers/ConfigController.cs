using DuPont.Models.Dtos.Background.Config;
using DuPont.Extensions;
using DuPont.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Models.Models;
using AutoMapper;

namespace DuPont.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly ISysSetting _systemSettingService;
        public ConfigController(IPermissionProvider permissionManage,
            IAdminUser adminUserRepository, ISysSetting systemSettingService)
            : base(permissionManage, adminUserRepository)
        {
            _systemSettingService = systemSettingService;
        }

        #region "系统配置项列表"
        [HttpPost]
        public ActionResult List()
        {
            CheckPermission();
            using (var result = new ResponseResult<List<SystemSettingViewModel>>())
            {
                var settingList = _systemSettingService.GetAll();
                result.Entity = Mapper.Map<List<SystemSettingViewModel>>(settingList);
                return ResponseSuccessWithJson<List<SystemSettingViewModel>>(result, "获取配置项列表成功!");
            }
        }
        #endregion

        #region "配置项详情"
        [HttpPost]
        public ActionResult Detail(int id)
        {
            CheckPermission();
            using (var result = new ResponseResult<SystemSettingViewModel>())
            {
                var setting = _systemSettingService.GetByKey(id);
                if (setting == null)
                {
                    return ResponseErrorWithJson<SystemSettingViewModel>(result, "指定的配置项不存在!");
                }

                result.Entity = Mapper.Map<SystemSettingViewModel>(setting);
                return ResponseSuccessWithJson<SystemSettingViewModel>(result, "获取配置项信息成功!");
            }
        }
        #endregion

        #region "更新配置项"
        /// <summary>
        /// 更新配置项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(SystemSettingViewModel input)
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                if (input.ID <= 0 || input.SETTING_VALUE.IsNullOrEmpty())
                {
                    return ResponseErrorWithJson<object>(result, "参数有误!请检查!");
                }

                _systemSettingService.Update(p => p.ID == input.ID, t => new T_SYS_SETTING
                {
                    SETTING_VALUE = input.SETTING_VALUE,
                    UPDATE_DATE = DateTime.Now
                });

                return ResponseSuccessWithJson<object>(result, "更新配置项成功!");
            }
        }
        #endregion
    }
}