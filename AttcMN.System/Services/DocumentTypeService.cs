using AttcMN.System.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Services
{
    public class DocumentTypeService:BaseService<DocumentType, DocumentTypeDto>, ITransient
    {
        private readonly ILogger<DocumentTypeService> _logger;
        private readonly DocumentTypeRepository _documentTypeRepository;
        public DocumentTypeService(ILogger<DocumentTypeService> logger,
            DocumentTypeRepository documentTypeRepository)
        {
            BaseRepo = documentTypeRepository;
            _logger = logger;
            _documentTypeRepository = documentTypeRepository;
        }
        public async Task<DocumentType> GetAsync(long? documentTypeId)
        {
            var entity = await base.FirstOrDefaultAsync(e => e.DocTypeId == documentTypeId);
            return entity;
        }
        public async Task<bool> InsertDocumentTypeAsync(DocumentTypeDto data)
        {
            return await _documentTypeRepository.InsertAsync(data);
        }
        public async Task<int> UpdateDocumentTypeAsync(DocumentTypeDto documentTypeDto)
        {
            return await _documentTypeRepository.UpdateAsync(documentTypeDto, true);
        }
        public async Task<bool> DeleteByDocTypeIDAsync(int docTypeId)
        {
            return await _documentTypeRepository.DeleteDocumentTypeById(docTypeId) > 0;
        }
    }
}
