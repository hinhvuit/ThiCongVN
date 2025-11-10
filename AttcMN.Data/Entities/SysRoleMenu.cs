using SqlSugar;

namespace AttcMN.Data.Entities
{
    /// <summary>
    ///  閫????” 撖寡情 sys_role_menu
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:53
    /// </summary>
    [SugarTable("sys_role_menu", "閫????”")]
    public class SysRoleMenu : BaseEntity
    {
        /// <summary>
        /// 閫ID (role_id)
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "閫ID")]
        public long RoleId { get; set; }
        /// <summary>
        /// ??ID (menu_id)
        /// </summary>
        [SugarColumn(ColumnName = "menu_id", ColumnDescription = "??ID")]
        public long MenuId { get; set; }
    }
}

