using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Repositories
{
    public class StorageRepository : BaseRepository<Storage, StorageDto>
    {
        public StorageRepository(ISqlSugarRepository<Storage> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }
        public override ISugarQueryable<StorageDto> DtoQueryable(StorageDto dto)
        {
            return Repo.AsQueryable()
           .WhereIF(!string.IsNullOrEmpty(dto.FilePath), (c) => c.FilePath == dto.FilePath)
           .WhereIF(!string.IsNullOrEmpty(dto.OldName), (c) => c.OldName!.Contains(dto.OldName!))
           .WhereIF(!string.IsNullOrEmpty(dto.ContentType), (c) => c.ContentType!.Contains(dto.ContentType!))
           .Where((c) => !c.IsDeleted)
           .WhereIF(dto.Params.BeginTime != null, (u) => u.CreateTime >= dto.Params.BeginTime)
           .WhereIF(dto.Params.EndTime != null, (u) => u.CreateTime <= dto.Params.EndTime)
           .Select((c) => new StorageDto
           {
               CreateBy = c.CreateBy,
               CreateTime = c.CreateTime,
               FileId = c.FileId,
               FilePath = c.FilePath,

               OldName = c.OldName,
               FileSize = c.FileSize,
               ContentType = c.ContentType,
               IsDeleted = c.IsDeleted
           });
        }

        public override ISugarQueryable<Storage> Queryable(StorageDto dto)
        {
            return Repo.AsQueryable()
           .WhereIF(!string.IsNullOrEmpty(dto.FilePath), (c) => c.FilePath == dto.FilePath)
           .WhereIF(!string.IsNullOrEmpty(dto.OldName), (c) => c.OldName!.Contains(dto.OldName!))
           .WhereIF(!string.IsNullOrEmpty(dto.ContentType), (c) => c.ContentType!.Contains(dto.ContentType!))
           .Where((c) => !c.IsDeleted)
           .WhereIF(dto.Params.BeginTime != null, (u) => u.CreateTime >= dto.Params.BeginTime)
           .WhereIF(dto.Params.EndTime != null, (u) => u.CreateTime <= dto.Params.EndTime);
        }
        public async Task<Storage> GetByFileIdAsync(Guid fileId)
        {
            return await Repo.AsQueryable()
                .Where(s => s.FileId == fileId && !s.IsDeleted)
                .FirstAsync();
        }
        public async Task MarkAsDeletedAsync(Guid fileId)
        {
            var storage = await GetByFileIdAsync(fileId);
            if (storage != null)
            {
                storage.IsDeleted = true;
                await Repo.UpdateAsync(storage);
            }
        }
        public async Task<List<Storage>> GetAllActiveFilesAsync()
        {
            return await Repo.AsQueryable()
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }
        public async Task<long> GetTotalActiveFileSizeAsync()
        {
            return await Repo.AsQueryable()
                .Where(s => !s.IsDeleted)
                .SumAsync(s => s.FileSize);
        }
        public async Task RestoreFileAsync(Guid fileId)
        {
            var storage = await GetByFileIdAsync(fileId);
            if (storage != null && storage.IsDeleted)
            {
                storage.IsDeleted = false;
                await Repo.UpdateAsync(storage);
            }
        }
        public async Task<long> GetActiveFilesCountAsync()
        {
            return await Repo.AsQueryable()
                .Where(s => !s.IsDeleted)
                .CountAsync();
        }
    }
}
