using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SqlSugar.SugarTable("form_statussign", "Form Signer Status")]
    public class Form_Statussign:BaseEntity
    {
        [SqlSugar.SugarColumn(ColumnName = "status_id", IsPrimaryKey = true, IsIdentity = true)]
        public long StatusId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "id_app", ColumnDescription = "id application")]
        public long IdApp { get; set; } = 0;
        [SqlSugar.SugarColumn(ColumnName = "form_id", ColumnDescription = "form id")]
        public int FormId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "signer_id", ColumnDescription = "signer Id")]
        public long SignerId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "signer_email", ColumnDescription = "signer Email")]
        public string SignerEmail { get; set; } = string.Empty;
        [SqlSugar.SugarColumn(ColumnName = "status_sign", ColumnDescription = "status sign")]
        public string StatusSign { get; set; } = string.Empty;
        [SqlSugar.SugarColumn(ColumnName = "sign_sort", ColumnDescription = "sign order")]
        public int SignSort { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "sign_rolename")]
        public string SignRolename { get; set; } = string.Empty;
    }
}
