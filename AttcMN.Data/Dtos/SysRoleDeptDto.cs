using System.Collections.Generic;
namespace AttcMN.Data.Dtos
{
    /// <summary>
    ///  閫??典?” 撖寡情 sys_role_dept
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    public class SysRoleDeptDto : BaseDto
    {
        /// <summary>
        /// 閫ID
        /// </summary>
        public long? RoleId { get; set; }
        /// <summary>
        /// ?券ID
        /// </summary>
        public long? DeptId { get; set; }
    }
}

