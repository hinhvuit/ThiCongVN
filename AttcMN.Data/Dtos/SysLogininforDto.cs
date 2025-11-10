namespace AttcMN.Data.Dtos
{
    /// <summary>
    ///  蝟餌?霈輸霈啣? 撖寡情 sys_logininfor
    ///  author ruoyi
    ///  date   2023-08-22 10:07:36
    /// </summary>
    public class SysLogininforDto : BaseDto
    {
        /// <summary>
        /// 霈輸ID
        /// </summary>
        public long InfoId { get; set; }

        /// <summary>
        /// ?冽韐血
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// ?餃?IP?啣?
        /// </summary>
        public string? Ipaddr { get; set; }

        /// <summary>
        /// ?餃??啁
        /// </summary>
        public string? LoginLocation { get; set; }

        /// <summary>
        /// 瘚??函掩??
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// ??蝟餌?
        /// </summary>
        public string? Os { get; set; }

        /// <summary>
        /// ?餃??嗆?0?? 1憭梯揖嚗?
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// ?內瘨
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// 霈輸?園
        /// </summary>
        public DateTime? LoginTime { get; set; }
    }
}

