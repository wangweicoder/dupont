using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_BUSINESS_PUBLISHED_DEMAND
    {
        public long Id { get; set; }
        public int DemandTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> CropId { get; set; }
        public int PublishStateId { get; set; }
        public string ExpectedDate { get; set; }
        public string TimeSlot { get; set; }
        public Nullable<decimal> ExpectedStartPrice { get; set; }
        public Nullable<decimal> ExpectedEndPrice { get; set; }
        public int AcquisitionWeightRangeTypeId { get; set; }
        public int FirstWeight { get; set; }
        public string Brief { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public string DetailedAddress { get; set; }
        public long CreateUserId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> ModifiedUserId { get; set; }
        public Nullable<long> DeletedUserId { get; set; }
        public Nullable<System.DateTime> DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
    }
}
