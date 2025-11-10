using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SugarTable("Worker")]
    public class Worker : UserBaseEntity
    {
        [SugarColumn(ColumnName = "work_id", IsPrimaryKey = true, IsIdentity = true)]
        public long WorkerId { get; set; }
        [SugarColumn(ColumnName = "emp_no", ColumnDescription = "emp_no")]
        public string EmpNo { get; set; } = string.Empty;
        [SugarColumn(ColumnName = "emp_name", ColumnDescription = "emp_name")]
        public string EmpName { get; set; } = string.Empty;
        [SugarColumn(ColumnName = "dept_id", ColumnDescription = "dept_id")]
        public int DeptId { get; set; }
        [SugarColumn(ColumnName = "idno", ColumnDescription = "idno")]
        public string Idno { get; set; } = string.Empty;
        [SugarColumn(ColumnName = "insure_file", ColumnDescription = "insure_file")]
        public Guid InsureFile { get; set; }
        [SugarColumn(ColumnName = "work_file", ColumnDescription = "work_file")]
        public Guid WorkFile { get; set; }
        [SugarColumn(ColumnName = "is_deleted")]
        public bool IsDeleted { get; set; }
        [SugarColumn(ColumnName = "remark", ColumnDescription = "remark")]
        public string Remark { get; set; } = string.Empty;
        [SugarColumn(ColumnName = "device_file", ColumnDescription = "device_file")]
        public Guid DeviceFile { get; set; }
        [SugarColumn(ColumnName = "safety2_file", ColumnDescription = "safety2_file")]
        public Guid Safety2File { get; set; }
        [SugarColumn(ColumnName = "safety2_enddate", ColumnDescription = "safety2_enddate")]
        public DateTime Safety2Enddate { get; set; }
        [SugarColumn(ColumnName = "safety3_file", ColumnDescription = "safety3_file")]
        public Guid Safety3File { get; set; }
        [SugarColumn(ColumnName = "safety3_enddate", ColumnDescription = "safety3_enddate")]
        public DateTime Safety3Enddate { get; set; }
        [SugarColumn(ColumnName = "safety4_file", ColumnDescription = "safety4_file")]
        public Guid Safety4File { get; set; }
        [SugarColumn(ColumnName = "safety4_redate", ColumnDescription = "safety4_redate")]
        public DateTime Safety4Redate { get; set; }
        [SugarColumn(ColumnName = "elec_file", ColumnDescription = "elec_file")]
        public Guid ElecFile { get; set; }
        [SugarColumn(ColumnName = "heathy_file", ColumnDescription = "heathy_file")]
        public Guid HeathyFile { get; set; }
        [SugarColumn(ColumnName = "protec_file", ColumnDescription = "protec_file")]
        public Guid ProtecFile { get; set; }
        [SugarColumn(ColumnName = "worker_image", ColumnDescription = "worker_image")]
        public Guid WorkerImage { get; set; }
        [SugarColumn(ColumnName = "fac_id")]
        public int FacId { get; set; }
        }
}
