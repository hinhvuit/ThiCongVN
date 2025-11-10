using AttcMN.Common.Constants;
using AttcMN.Common.Utils;
using AttcMN.Framework.Cache;
using AttcMN.Framework.Exceptions;
using AttcMN.Framework.Interceptors;
using AttcMN.System.Repositories;
using AttcMN.System.Slave.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Services
{
    public class FactoryService : BaseService<Factory,FactoryDto>,ITransient
    {
        private readonly ILogger<SysConfigService> _logger;
        private readonly ICache _cache;
        private readonly FactoryRepository _factoryRepository;
        public FactoryService(ILogger<SysConfigService> logger,
        ICache cache,
        FactoryRepository factoryRepository)
        {
            BaseRepo = factoryRepository;

            _logger = logger;
            _cache = cache;
            _factoryRepository = factoryRepository;

        }
        public async Task<int> InsertFactoryAsync(FactoryDto factory)
        {
            return await _factoryRepository.InsertFactoryAsync(factory);
        }
        public async Task<Factory> GetFactoryByIdAsync(int facId)
        {
            return await _factoryRepository.GetFactoryByIdAsync(facId);
        }
        
        /// <summary>
        /// 根据用户信息获取工厂列表
        /// </summary>
        /// <param name="dto">用户DTO（必须包含FactoryIds）</param>
        /// <returns>工厂列表</returns>
        public async Task<List<Factory>> GetFactoriesByUserIdAsync(SysUserDto dto)
        {
            List<Factory> factories = new List<Factory>();
            
            // 检查dto和UserId是否为空
            if (dto == null || dto.UserId == null)
                return factories;
            
            // 检查FactoryIds是否为空或空列表
            if (dto.FactoryIds != null && dto.FactoryIds.Count > 0)
            {
                factories = await _factoryRepository.GetFactoryByFacIdsAsync(dto.FactoryIds);
            }
            
            return factories;
        }
        
        public async Task<List<FactoryDto>> GetFactoryListAsync(FactoryDto dto)
        {
            return await _factoryRepository.DtoQueryable(dto).ToListAsync();
        }
        public async Task<List<FactoryDto>> GetAllFactoriesAsync()
        {
            return await _factoryRepository.DtoQueryable(new FactoryDto { }).ToListAsync();
        }
        public async Task<bool> UpdateFactoryAsync(Factory factory)
        {
            return await _factoryRepository.UpdateFactoryAsync(factory);
        }
        public bool DeleteFactoryByUserFacIDs(int[] facids)
        {
            return facids.All(facid => _factoryRepository.DeleteFactoryByUserFacID(facid) > 0);
        }
        public async Task<int> DeleteFactoryAsync(int facId)
        {
            return await _factoryRepository.DeleteFactoryAsync(facId);
        }

    }
}
