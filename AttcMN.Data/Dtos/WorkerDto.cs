using AttcMN.Data.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace AttcMN.Data.Dtos
{
    public class WorkerDto : BaseDto
    {
        public long WorkerId { get; set; }

        [Excel(Name = "EmpNo")]
        [MaxLength(10, ErrorMessage = "EmpNo length cannot exceed 10 characters")]
        public string? EmpNo { get; set; }

        [Excel(Name = "EmpName")]
        [MaxLength(100, ErrorMessage = "EmpName length cannot exceed 100 characters")]
        public string? EmpName { get; set; }

        public int? DeptId { get; set; }
        public int? FacId { get; set; }

        [MaxLength(30, ErrorMessage = "Idno length cannot exceed 30 characters")]
        public string? Idno { get; set; }

        public Guid? InsureFile { get; set; }
        public Guid? WorkFile { get; set; }

        public bool? IsDeleted { get; set; }

        [MaxLength(1000)]
        public string? Remark { get; set; }

        public Guid? DeviceFile { get; set; }

        public Guid? Safety2File { get; set; }
        public DateTime? Safety2Enddate { get; set; }

        public Guid? Safety3File { get; set; }
        public DateTime? Safety3Enddate { get; set; }

        public Guid? Safety4File { get; set; }
        public DateTime? Safety4Redate { get; set; }

        public Guid? ElecFile { get; set; }
        public Guid? HeathyFile { get; set; }
        public Guid? ProtecFile { get; set; }

        public Guid? WorkerImage { get; set; }
    }
}
