using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class T_MACHINE_DEMANDTYPE_RELATION
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 机器编号
        /// </summary>
        [Key, Column(Order = 1)]
        public int MachineId { get; set; }

        /// <summary>
        /// 服务类型编号
        /// </summary>
        [Key, Column(Order = 2)]
        public int DemandTypeId { get; set; }
    }
}
