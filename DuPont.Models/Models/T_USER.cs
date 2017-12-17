using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuPont.Models.Models
{
    public partial class T_USER
    {
        public T_USER()
        {
            Questions = new List<T_QUESTION>();
            Notifications = new List<T_NOTIFICATION>();
            ReceivedPrivateNotifications = new List<T_SEND_NOTIFICATION_RESULT>();
            BookFarmList = new List<T_FARM_BOOKING>();
            FarmerDemandList = new List<T_FARMER_PUBLISHED_DEMAND>();
            CreateTime = DateTime.Now;
            IsDeleted = false;
            //UserType = 0;
        }
        public long Id { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string LoginToken { get; set; }
        public string AvartarUrl { get; set; }
        [SQLValidate]
        public string UserName { get; set; }
        [SQLValidate]
        public string Province { get; set; }
        [SQLValidate]
        public string City { get; set; }
        public Nullable<int> DPoint { get; set; }
        [SQLValidate]
        public string Region { get; set; }
        [SQLValidate]
        public string Township { get; set; }
        [SQLValidate]
        public string Village { get; set; }
        public string DetailedAddress { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> ModifiedUserId { get; set; }
        public Nullable<long> DeletedUserId { get; set; }
        public Nullable<System.DateTime> DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
        public string SmsCode { get; set; }
        public string LoginUserName { get; set; }

        /// <summary>
        /// ���һ�ε�¼��ʱ��
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// ���һ���޸������ʱ��
        /// </summary>
        public DateTime? LastUpdatePwdTime { get; set; }

        /// <summary>
        /// IOS�豸���
        /// </summary>
        public string IosDeviceToken { get; set; }

        public virtual List<T_QUESTION> Questions { get; set; }
        public virtual List<T_NOTIFICATION> Notifications { get; set; }
        public virtual List<T_SEND_NOTIFICATION_RESULT> ReceivedPrivateNotifications { get; set; }
        public virtual List<T_FARM_BOOKING> BookFarmList { get; set; }

        /// <summary>
        /// ��ũ������������
        /// </summary>
        public virtual List<T_FARMER_PUBLISHED_DEMAND> FarmerDemandList { get; set; }

        /// <summary>
        ///  �û�����
        /// </summary>
        //public int? UserType { get; set; }

        /// <summary>
        /// �û��ǳ�
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// �û�����
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// �û���Դ
        /// </summary>
        public int SourceType { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string WeatherCity { get; set; }
    }
}
