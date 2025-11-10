

namespace AttcMN.Data.Entities
{
    [SqlSugar.SugarTable("form_processsign", "Form Process Sign")]
    public class Form_Processsign:BaseEntity
    {
        [SqlSugar.SugarColumn(ColumnName = "proc_id", IsPrimaryKey = true, IsIdentity = true)]
        public long ProcesssignId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "form_id", ColumnDescription = "Form ID")]
        public int FormId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "sign_memo", ColumnDescription = "Memo Sign")]
        public string SignMemo { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "status_id", ColumnDescription = "status ID")]
        public int StatusId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "sign_status", ColumnDescription = "sign status")]
        public string SignStatus { get; set; } = string.Empty;
        [SqlSugar.SugarColumn(ColumnName = "sign_time", ColumnDescription = "sign time")]
        public DateTime SignTime { get; set; }
    }
}
