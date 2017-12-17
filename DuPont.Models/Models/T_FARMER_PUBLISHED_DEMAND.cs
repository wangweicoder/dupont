using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_FARMER_PUBLISHED_DEMAND
    {
        public T_FARMER_PUBLISHED_DEMAND()
        {
            IsOpen = true;
            Operators = new List<T_USER>();
        }

        public long Id { get; set; }
        public int DemandTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public int PublishStateId { get; set; }
        public int CropId { get; set; }
        public Nullable<int> VarietyId { get; set; }
        public int AcresId { get; set; }
        public string Brief { get; set; }
        public string ExpectedDate { get; set; }
        public string TimeSlot { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public string DetailedAddress { get; set; }
        public Nullable<decimal> ExpectedStartPrice { get; set; }
        public Nullable<decimal> ExpectedEndPrice { get; set; }
        public long CreateUserId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> ModifiedUserId { get; set; }
        public Nullable<long> DeletedUserId { get; set; }
        public Nullable<System.DateTime> DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }

        /// <summary>
        /// 是否是公开的（否则是发给指定农机手的）
        /// </summary>
        public bool IsOpen { get; set; }

        public virtual List<T_USER> Operators { get; set; }
    }
}
