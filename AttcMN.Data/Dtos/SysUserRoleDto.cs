namespace AttcMN.Data.Dtos
{
    /// <summary>
    ///  ?冽???脣?” 撖寡情 sys_user_role
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    public class SysUserRoleDto : BaseDto
    {
        /// <summary>
        /// ?冽ID
        /// </summary>
        public long UserId { get; set; }
        public List<long> UserIds { get; set; }
        /// <summary>
        /// 閫ID
        /// </summary>
        public long RoleId { get; set; }
    }
}

