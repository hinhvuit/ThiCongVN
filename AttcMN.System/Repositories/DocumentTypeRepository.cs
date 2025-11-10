using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Repositories
{
    public class DocumentTypeRepository : BaseRepository<DocumentType, DocumentTypeDto>
    {
        public DocumentTypeRepository(ISqlSugarRepository<DocumentType> sqlSugarRepository) { 
            Repo = sqlSugarRepository;
        }
        public override ISugarQueryable<DocumentTypeDto> DtoQueryable(DocumentTypeDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.DocTypeId > 0, (t) => t.DocTypeId == dto.DocTypeId && t.IsDeleted == false)
                .Select((t) => new DocumentTypeDto
                {
                    DocTypeId = t.DocTypeId,
                    DocTypeName = t.DocTypeName
                });
        }

        public override ISugarQueryable<DocumentType> Queryable(DocumentTypeDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.DocTypeId > 0, (t) => t.DocTypeId == dto.DocTypeId);
        }
        public List<DocumentTypeDto> GetDocumentTypeList()
        {
            return DtoQueryable(new DocumentTypeDto { }).ToList();
        }
        public async Task<DocumentType> GetDocumentTypeByIdAsync(int doctypeid)
        {
            return await this.FirstOrDefaultAsync(f => f.DocTypeId == doctypeid);
        }
        public async Task<DocumentType> GetDocumentTypeByNameAsync(string doctypeName)
        {
            return await this.FirstOrDefaultAsync(f => f.DocTypeName == doctypeName);
        }
        public async Task<int> DeleteDocumentTypeById(int doctype)
        {
            return Repo.Delete(r => r.DocTypeId == doctype);
        }
        public async Task<List<DocumentType>> GetListByCreateByAsync(string createBy)
        {
            if (string.IsNullOrWhiteSpace(createBy))
                return new List<DocumentType>();
            return await Repo.AsQueryable()
                             .Where(f => f.CreateBy == createBy && f.IsDeleted == false)
                             .ToListAsync();
        }
        public async Task<List<DocumentType>> GetListByCreateByAsync(long userId)
        {
            return await GetListByCreateByAsync(userId.ToString());
        }
        
    }
}
