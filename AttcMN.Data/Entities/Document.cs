using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SugarTable("document", "Manager Document")]
    public class Document: UserBaseEntity
    {
        [SugarColumn(ColumnName = "doc_Id", IsPrimaryKey = true,IsIdentity =true)]
        public long DocId { get; set; }

        [SugarColumn(ColumnName = "doc_type_id")]
        public int DocTypeId { get; set; }

        [SugarColumn(ColumnName = "doc_name")]
        public string DocName { get; set; }

        [SugarColumn(ColumnName = "file_id")]
        public Guid FileId { get; set; }

        [SugarColumn(ColumnName = "construc_id")]
        public int ConstrucId { get; set; } = 0;

        [SugarColumn(ColumnName = "is_deleted")]
        public bool IsDeleted { get; set; }
        
        [SugarColumn(ColumnName = "vendor_id")]
        public int VendorId { get; set; } = 0;

        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }
    }
}
