using AttcMN.Data.Attributes;

namespace AttcMN.Data.Dtos
{
    /// <summary>
    ///  ???亙?霈啣? 撖寡情 sys_oper_log
    ///  author ruoyi
    ///  date   2023-09-28 12:38:39
    /// </summary>
    public class SysOperLogDto : BaseDto
    {
        /// <summary>
        /// ?亙?銝駁
        /// </summary>
        [Excel(Name = "??摨")]
        public long OperId { get; set; }

        /// <summary>
        /// 璅∪???
        /// </summary>
        [Excel(Name = "??璅∪?")]
        public string? Title { get; set; }

        /// <summary>
        /// 銝蝐餃?嚗??嗅? 1?啣? 2靽格 3?嚗?
        /// </summary>
        public int? BusinessType { get; set; }

        [Excel(Name = "銝蝐餃?")]
        public string? BusinessTypeDesc { get; set; }

        /// <summary>
        /// ?寞??妍
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// 霂瑟??孵?
        /// </summary>
        public string? RequestMethod { get; set; }
        /// <summary>
        /// ??蝐餃嚗??嗅? 1??冽 2?蝡舐?瘀?
        /// </summary>
        public int? OperatorType { get; set; }
        /// <summary>
        /// ??鈭箏?
        /// </summary>
        public string? OperName { get; set; }
        /// <summary>
        /// ?券?妍
        /// </summary>
        public string? DeptName { get; set; }
        /// <summary>
        /// 霂瑟?URL
        /// </summary>
        public string? OperUrl { get; set; }
        /// <summary>
        /// 銝餅?啣?
        /// </summary>
        public string? OperIp { get; set; }
        /// <summary>
        /// ???啁
        /// </summary>
        public string? OperLocation { get; set; }
        /// <summary>
        /// 霂瑟??
        /// </summary>
        public string? OperParam { get; set; }
        /// <summary>
        /// 餈??
        /// </summary>
        public string? JsonResult { get; set; }
        /// <summary>
        /// ???嗆?0甇?虜 1撘虜嚗?
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// ?秤瘨
        /// </summary>
        public string? ErrorMsg { get; set; }
        /// <summary>
        /// ???園
        /// </summary>
        public DateTime? OperTime { get; set; }
        /// <summary>
        /// 瘨??
        /// </summary>
        public long? CostTime { get; set; }
    }
}

