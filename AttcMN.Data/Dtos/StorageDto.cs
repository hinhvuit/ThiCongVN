using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.Data.Dtos
{
    public class StorageDto : BaseDto
    {
        public Guid FileId { get; set; }
        public string? OldName { get; set; }
        public string? FilePath { get; set; }
        public long FileSize { get; set; }
        public string? ContentType { get; set; }
        public bool IsDeleted { get; set; }
        public string? ViewUrl { get; set; }
    }
}
