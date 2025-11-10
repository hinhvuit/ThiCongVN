using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SugarTable("factory", "All factory")]
    public class Factory:CreateUserBaseEntity
    {
        [SugarColumn(ColumnName = "fac_id", IsPrimaryKey = true, IsIdentity = true)]
        public int FacId { get; set; }

        [SugarColumn(ColumnName = "fac_name", ColumnDescription = "Fac Name")]
        public string? FacName { get; set; }

        [SugarColumn(ColumnName = "fac_addr", ColumnDescription = "Fac Address")]
        public string? FacAddress { get; set; }

        [SugarColumn(ColumnName = "fac_short", ColumnDescription = "Short Name")]
        public string? FacShort { get; set; }

        [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "is deleted")]
        public bool IsDeleted { get; set; }
    }
}
