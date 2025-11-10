using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.Data.Dtos
{
    public class DocumentTypeDto:BaseDto
    {
        public int DocTypeId { get; set; }
        public string? DocTypeName { get; set; }
    }
}
