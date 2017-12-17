
using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Linq;

namespace DuPont.Repository
{
    public class SysSettingRepository : BaseRepository<T_SYS_SETTING>, ISysSetting
    {

        public T_SYS_SETTING GetSetting(string settingId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_SYS_SETTING.Where(set => set.SETTING_ID == settingId).FirstOrDefault();
            }
        }
    }
}
