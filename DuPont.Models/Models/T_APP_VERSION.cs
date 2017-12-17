using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_APP_VERSION
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public int VersionCode { get; set; }
        public string DownloadURL { get; set; }
        public string ChangeLog { get; set; }
        public int CREATE_USER { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public Nullable<int> UPDATE_USER { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string Platform { get; set; }
        public bool IsOpen { get; set; }
    }
}
