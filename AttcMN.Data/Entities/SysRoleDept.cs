using SqlSugar;

namespace AttcMN.Data.Entities
{
    /// <summary>
    ///  閫??典?” 撖寡情 sys_role_dept
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    [SugarTable("sys_role_dept", "閫??典?”")]
    public class SysRoleDept : BaseEntity
    {
        /// <summary>
        /// 閫ID (role_id)
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "閫ID")]
        public long RoleId { get; set; }
        /// <summary>
        /// ?券ID (dept_id)
        /// </summary>
        [SugarColumn(ColumnName = "dept_id", ColumnDescription = "?券ID")]
        public long DeptId { get; set; }
    }
}

