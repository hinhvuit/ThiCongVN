using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SugarTable("document_type", "Manager Document Type")]
    public class DocumentType:CreateUserBaseEntity
    {
        [SugarColumn(ColumnName = "doc_type_id", IsPrimaryKey = true, IsIdentity = true)]
        public int DocTypeId { get; set; }
        [SugarColumn(ColumnName = "doc_type_name")]
        public string DocTypeName { get; set; }
        [SugarColumn(ColumnName = "is_deleted")]
        public bool IsDeleted { get; set; }
    }
}
