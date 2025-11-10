using System.Collections.Generic;
namespace AttcMN.Data.Dtos
{
    /// <summary>
    ///  ?冽銝?雿?” 撖寡情 sys_user_post
    ///  author AttcMN.net
    ///  date   2023-08-23 09:43:50
    /// </summary>
    public class SysUserPostDto : BaseDto
    {
        /// <summary>
        /// ?冽ID
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 撗?ID
        /// </summary>
        public long? PostId { get; set; }
    }
}

