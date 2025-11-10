using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SugarTable("form", "Manager Form")]
    public class Form:CreateUserBaseEntity
    {
        [SugarColumn(ColumnName = "form_id", IsPrimaryKey = true, IsIdentity = true)]
        public int FormId { get; set; }
        [SugarColumn(ColumnName = "form_name")]
        public string FormName { get; set; }
        [SugarColumn(ColumnName = "is_deleted")]
        public bool IsDeleted { get; set; }
        [SugarColumn(ColumnName = "form_db")]
        public string FormDb { get; set; }
    }
}
