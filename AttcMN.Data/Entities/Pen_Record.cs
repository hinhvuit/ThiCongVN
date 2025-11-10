using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SqlSugar.SugarTable("pen_record", "Pen Record")]
    public class Pen_Record: UserBaseEntity
    {
        [SqlSugar.SugarColumn(ColumnName = "pen_id", IsPrimaryKey = true, IsIdentity = true)]
        public long PenId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "dep_id", ColumnDescription = "Dept ID")]
        public int DepId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "pen_content", ColumnDescription = "Pen Content")]
        public string PenContent { get; set; } = string.Empty;
        [SqlSugar.SugarColumn(ColumnName = "status", ColumnDescription = "Status")]
        public string Status { get; set; } = string.Empty;
        [SqlSugar.SugarColumn(ColumnName = "fac_id", ColumnDescription = "Fac Id")]
        public int FacId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "pen_file", ColumnDescription = "pen file")]
        public Guid PenFile { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "pen_address")]
        public string PenAddress { get; set; } = string.Empty;
        [SqlSugar.SugarColumn(ColumnName = "is_deleted")]
        public bool IsDeleted { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "pen_date")]
        public DateTime PenDate { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "pen_memo")]
        public string PenMemo { get; set; } = string.Empty;
        
    }
}
