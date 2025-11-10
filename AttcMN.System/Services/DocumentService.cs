using AttcMN.System.Repositories;
using AttcMN.System.Slave.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Services
{
    public class DocumentService : BaseService<Document, DocumentDto>, ITransient
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly DocumentRepository _documentRepository;
        private readonly DocumentTypeRepository _documentTypeRepository;
        private readonly FactoryRepository _factoryRepository;
        private readonly SysDeptRepository _sysDeptRepository;

        public DocumentService(ILogger<DocumentService> logger,
            DocumentRepository documentRepository, DocumentTypeRepository documentTypeRepository, FactoryRepository factoryRepository, SysDeptRepository sysDeptRepository)
        {
            BaseRepo = documentRepository;
            _logger = logger;
            _documentRepository = documentRepository;
            _documentTypeRepository = documentTypeRepository;
            _factoryRepository = factoryRepository;
            _sysDeptRepository = sysDeptRepository;
        }
        public async Task<Document> GetAsync(long? documentId)
        {
            var entity = await base.FirstOrDefaultAsync(e => e.DocId == documentId);
            return entity;
        }
        public async Task<bool> InsertDocumentAsync(DocumentDto data)
        {
            return await _documentRepository.InsertDocumentAsync(data)>0;
        }
        public async Task<bool> UpdateDocumentFileAsync(Guid fileId, long docId)
        {
            return await _documentRepository.UpdateDocFileAsync(fileId, docId) > 0;
        }
        public async Task<int> UpdateDocumentAsync(DocumentDto documentDto)
        {
            return await _documentRepository.UpdateAsync(documentDto, true);
        }
        public List<DocumentTypeDto> GetDocumentTypeList()
        {
            return _documentTypeRepository.GetDocumentTypeList();
        }
        public async Task<List<Document>> GetListByCreateByAsync(string createBy)
        {
            return await _documentRepository.GetListByCreateByAsync(createBy);
        }
        public async Task<List<Document>> GetListByCreateByAsync(long userId)
        {
            return await _documentRepository.GetListByCreateByAsync(userId);
        }
        public async Task<bool> DeleteByDocIDAsync(long docId)
        {
            return await _documentRepository.DeleteDocumentById(docId) > 0;
        }
        public virtual async Task<List<DocumentDto>> GetlistAsync(DocumentDto dto)
        {
            return await _documentRepository.DtoQueryable(dto).ToListAsync();
        }
        public List<DocumentDto> GetDocumentList()
        {
            return _documentRepository.GetDocumentList();
        }
        public async Task<List<Document>> GetListByFacIdAsync(int facId)
        {
            return await _documentRepository.GetListByFacIdAsync(facId);
        }
        public virtual async Task<SqlSugarPagedList<Document>> GetPagedDocumentListAsync(DocumentDto dto)
        {
            return await _documentRepository.GetPagedListAsync(dto);
        }
    }
}
