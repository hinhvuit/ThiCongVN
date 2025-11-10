using SqlSugar;

namespace AttcMN.Data.Entities
{
    /// <summary>
    ///  ?冽銝?雿?” 撖寡情 sys_user_post
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:50
    /// </summary>
    [SugarTable("sys_user_post", "?冽銝?雿?”")]
    public class SysUserPost : BaseEntity
    {
        /// <summary>
        /// ?冽ID (user_id)
        /// </summary>
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "?冽ID")]
        public long UserId { get; set; }
        /// <summary>
        /// 撗?ID (post_id)
        /// </summary>
        [SugarColumn(ColumnName = "post_id", ColumnDescription = "撗?ID")]
        public long PostId { get; set; }
    }
}

