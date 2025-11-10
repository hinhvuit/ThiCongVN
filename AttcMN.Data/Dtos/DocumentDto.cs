using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.Data.Dtos
{
    public class DocumentDto:BaseDto
    {
        public long DocId { get; set; }
        public int? DocTypeId { get; set; }
        public string? DocName { get; set; }
        public Guid? FileId { get; set; }
        public int? FacId { get; set; } = 0;
        public bool? IsDeleted { get; set; }
        public int? VendorId { get; set; } = 0;
        public string? Remark { get; set; }
        public string? VendorName { get; set; }
        public DocumentTypeDto DocumentTypeDto { get; set; }

        // New: URLs for client to view or download the file
        public string? DownloadUrl { get; set; }
        public string? ViewUrl { get; set; }
    }
}
