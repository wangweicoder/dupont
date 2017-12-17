using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_USER_ROLE_RELATION
    {
        public T_USER_ROLE_RELATION()
        {
            Star = 0;
            TotalReplyCount = 0;
            TotalStarCount = 0;
        }
        /// <summary>
        /// �û����
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// ��ɫ���
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// �Ƿ�Ϊǰ̨ע���Ա
        /// </summary>
        public bool MemberType { get; set; }
        /// <summary>
        /// ��ɫ�ȼ�
        /// </summary>
        public Nullable<long> Star { get; set; }
        /// <summary>
        /// �ۼ���������
        /// </summary>
        public long? TotalStarCount { get; set; }
        /// <summary>
        /// �ۼ���������
        /// </summary>
        public long? TotalReplyCount { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public long AuditUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}
