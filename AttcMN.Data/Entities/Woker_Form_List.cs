using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SqlSugar.SugarTable("worker_form_list", "Worker Form List")]
    public class Worker_Form_List:CreateUserBaseEntity
    {
        [SqlSugar.SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public long WflId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "worke_id", ColumnDescription = "Worke ID")]
        public long WorkeId { get; set; }
        [SqlSugar.SugarColumn(ColumnName = "form_id", ColumnDescription = "Form ID")]
        public int FormId { get; set; }
    }
}
