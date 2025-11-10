using SqlSugar;

namespace AttcMN.Data.Entities
{
    /// <summary>
    ///  ?冽???脣?” 撖寡情 sys_user_role
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    [SugarTable("sys_user_role", "?冽???脣?”")]
    public class SysUserRole: BaseEntity
    {
        /// <summary>
        /// ?冽ID (user_id)
        /// </summary>
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "?冽ID")]
        public long UserId { get; set; }
        /// <summary>
        /// 閫ID (role_id)
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "閫ID")]
        public long RoleId { get; set; }
    }
}

