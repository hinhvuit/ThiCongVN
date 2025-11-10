using AttcMN.System.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Services
{
    public class StorageService : BaseService<Storage, StorageDto>, ITransient
    {
        private readonly ILogger<SysRoleService> _logger;
        private readonly StorageRepository _storageRepository;

        public StorageService(ILogger<SysRoleService> logger,
            StorageRepository storageRepository)
        {
            BaseRepo = storageRepository;

            _logger = logger;
            _storageRepository = storageRepository;
        }
        public async Task<Storage> GetByFileIdAsync(Guid fileId)
        {
            return await _storageRepository.GetByFileIdAsync(fileId);
        }
        public async Task MarkAsDeletedAsync(Guid fileId)
        {
            await _storageRepository.MarkAsDeletedAsync(fileId);
        }
        public async Task<SqlSugarPagedList<StorageDto>> GetPagedStorageListAsync(StorageDto dto)
        {
            return await _storageRepository.GetDtoPagedListAsync(dto);
        }
        public async Task<List<StorageDto>> GetStorageListAsync(StorageDto dto)
        {
            return await _storageRepository.GetDtoListAsync(dto);
        }
        public async Task<List<Storage>> GetAllActiveFilesAsync()
        {
            return await _storageRepository.GetAllActiveFilesAsync();
        }
        public async Task<long> GetActiveFilesCountAsync()
        {
            return await _storageRepository.GetActiveFilesCountAsync();
        }

    }
}
