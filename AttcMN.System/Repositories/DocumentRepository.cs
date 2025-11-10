using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Repositories
{
    public class DocumentRepository : BaseRepository<Document, DocumentDto>
    {
        public DocumentRepository(ISqlSugarRepository<Document> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }
        public override ISugarQueryable<DocumentDto> DtoQueryable(DocumentDto dto)
        {
            return Repo.AsQueryable()
                .LeftJoin<DocumentType>((t, f) => t.DocTypeId == f.DocTypeId)
                .WhereIF(dto.DocId > 0, (t) => t.DocId == dto.DocId && t.IsDeleted == false)
                .WhereIF(dto.DocTypeId.HasValue, (t) => t.DocTypeId == dto.DocTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(dto.DocName), (t) => t.DocName.Contains(dto.DocName))
                .WhereIF(dto.FacId.HasValue, (t) => t.ConstrucId == dto.FacId)
                .WhereIF(dto.VendorId.HasValue, (t) => t.VendorId == dto.VendorId)
                .Select((t, f) => new DocumentDto
                {
                    DocId = t.DocId,
                    DocTypeId = t.DocTypeId,
                    DocName = t.DocName,
                    FileId = t.FileId,
                    FacId = t.ConstrucId,
                    IsDeleted = t.IsDeleted,
                    VendorId = t.VendorId,
                    Remark = t.Remark,
                    DocumentTypeDto = new DocumentTypeDto
                    {
                        DocTypeId = f.DocTypeId,
                        DocTypeName = f.DocTypeName
                    }
                });
        }

        public override ISugarQueryable<Document> Queryable(DocumentDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.DocId > 0, (t) => t.DocId == dto.DocId)
                .WhereIF(dto.DocTypeId.HasValue, (t) => t.DocTypeId == dto.DocTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(dto.DocName), (t) => t.DocName.Contains(dto.DocName))
                .WhereIF(dto.FacId.HasValue, (t) => t.ConstrucId == dto.FacId)
                .WhereIF(dto.VendorId.HasValue, (t) => t.VendorId == dto.VendorId);
        }

        // Fill DTOs with file view/download URLs
        public override void FillRelatedData(IEnumerable<DocumentDto> entities)
        {
            if (entities == null) return;

            foreach (var dto in entities)
            {
                if (dto == null) continue;

                if (dto.FileId.HasValue && dto.FileId != Guid.Empty)
                {
                    // Relative endpoints exposed by StorageController
                    dto.DownloadUrl = $"/system/file/download/byid/{dto.FileId.Value}";
                    dto.ViewUrl = $"/system/file/view/{dto.FileId.Value}";
                }
                else
                {
                    dto.DownloadUrl = null;
                    dto.ViewUrl = null;
                }
            }

            // call base in case it does other work
            base.FillRelatedData(entities);
        }

        public async Task<Document> GetDocumentByIdAsync(long docId)
        {
            return await this.FirstOrDefaultAsync(f => f.DocId == docId);
        }
        public async Task<List<Document>> GetListByCreateByAsync(string createBy)
        {
            if (string.IsNullOrWhiteSpace(createBy))
                return new List<Document>();
            return await Repo.AsQueryable()
                             .Where(f => f.CreateBy == createBy && f.IsDeleted == false)
                             .ToListAsync();
        }
        public async Task<List<Document>> GetListByCreateByAsync(long userId)
        {
            return await GetListByCreateByAsync(userId.ToString());
        }
        public async Task<List<Document>> GetListByFacIdAsync(int facId)
        {
            return await Repo.AsQueryable()
                             .Where(f => f.ConstrucId == facId && f.IsDeleted == false)
                             .ToListAsync();
        }
        public async Task<List<Document>> GetListByVendorIdAsync(int vendorId)
        {
            return await Repo.AsQueryable()
                             .Where(f => f.VendorId == vendorId && f.IsDeleted == false)
                             .ToListAsync();
        }
        public async Task<int> DeleteDocumentById(long docId)
        {
            return  Repo.Delete(r => r.DocId == docId);
        }
        public int DeleteDocumentByFacId(int facId)
        {
            return Repo.Delete(r => r.ConstrucId == facId);
        }
        public int DeleteDocumentByVendorId(int vendorId)
        {
            return Repo.Delete(r => r.VendorId == vendorId);
        }
        public List<DocumentDto> GetDocumentList()
        {
            return DtoQueryable(new DocumentDto { }).ToList();
        }
        public List<DocumentDto> GetDocumentListByFacId(int facId)
        {
            return DtoQueryable(new DocumentDto { FacId = facId }).ToList();
        }
        public List<DocumentDto> GetDocumentListByFacIdandVenDorId(int facId, int vendorid)
        {
            return DtoQueryable(new DocumentDto { FacId = facId ,VendorId=vendorid}).ToList();
        }
        public List<DocumentDto> GetDocumentListByDocTypeID(int doctypeid)
        {
            return DtoQueryable(new DocumentDto { DocTypeId = doctypeid }).ToList();
        }
        public async Task<int> UpdateDocNameAsync(string docName,long docid)
        {
            return await base.Updateable()
                 .SetColumns(u => u.DocName == docName)
                 .Where(u => u.DocId == docid)
                 .ExecuteCommandAsync();
        }
        public async Task<int> UpdateDocFileAsync(Guid fileId, long docid)
        {
            return await base.Updateable()
                 .SetColumns(u => u.FileId == fileId)
                 .Where(u => u.DocId == docid)
                 .ExecuteCommandAsync();
        }

        /// <summary>
        /// Insert Document với custom mapping từ DocumentDto
        /// </summary>
        /// <param name="dto">DocumentDto chứa dữ liệu cần insert</param>
        /// <returns>Id của document vừa insert</returns>
        public async Task<long> InsertDocumentAsync(DocumentDto dto)
        {
            var entity = new Document
            {
                DocTypeId = dto.DocTypeId ?? 0,
                DocName = dto.DocName ?? string.Empty,
                FileId = dto.FileId ?? Guid.Empty,
                ConstrucId = dto.FacId ?? 0,
                IsDeleted = dto.IsDeleted ?? false,
                VendorId = dto.VendorId ?? 0,
                Remark = dto.Remark ?? string.Empty,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime ?? DateTime.Now
            };

            // Insert và trả về Id
            return await Repo.InsertReturnIdentityAsync(entity);
        }
    }
}
