using System.Collections.Generic;
namespace AttcMN.Data.Dtos
{
    /// <summary>
    ///  閫????” 撖寡情 sys_role_menu
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:53
    /// </summary>
    public class SysRoleMenuDto : BaseDto
    {
        /// <summary>
        /// 閫ID
        /// </summary>
        public long? RoleId { get; set; }
        /// <summary>
        /// ??ID
        /// </summary>
        public long? MenuId { get; set; }
    }
}

