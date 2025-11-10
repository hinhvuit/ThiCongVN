namespace AttcMN.System.Repositories
{
    public class FactoryRepository : BaseRepository<Factory, FactoryDto>
    {
        public FactoryRepository(ISqlSugarRepository<Factory> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }
        public override ISugarQueryable<Factory> Queryable(FactoryDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.FacId > 0, (t) => t.FacId == dto.FacId);
        }

        public override ISugarQueryable<FactoryDto> DtoQueryable(FactoryDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.FacId > 0, (t) => t.FacId == dto.FacId && t.IsDeleted == false)
                .Select((t) => new FactoryDto
                {
                    FacId = t.FacId,
                    FacName = t.FacName,
                    FacAddress = t.FacAddress,
                    ShortName = t.FacShort,
                    CreateBy=t.CreateBy,
                    CreateTime=t.CreateTime,
                });
        }
        public int DeleteFactoryByUserFacID(int facid)
        {
            return Repo.Delete(r => r.FacId == facid);
        }
        public List<FactoryDto> GetFactoryList()
        {
            return DtoQueryable(new FactoryDto { }).ToList();
        }
        public async Task<Factory> GetFactoryByIdAsync(int facid)
        {
            return await this.FirstOrDefaultAsync(f => f.FacId == facid);
        }
        public async Task<List<Factory>> GetFactoryByFacIdsAsync(List<int> facids)
        {
            // Guard clause: return empty list if input is null or empty
            if (facids == null || facids.Count == 0)
                return new List<Factory>();
                
            return await Repo.AsQueryable()
                             .Where(f => facids.Contains(f.FacId) && f.IsDeleted == false)
                             .ToListAsync();
        }
        public async Task<Factory> GetFactoryByShortNameAsync(string shortName)
        {
            return await this.FirstOrDefaultAsync(f => f.FacShort == shortName);
        }
        // Return factories created by the specified CreateBy string (e.g. user id stored as string or username)
        public async Task<List<Factory>> GetListByCreateByAsync(string createBy)
        {
            if (string.IsNullOrWhiteSpace(createBy))
                return new List<Factory>();

            return await Repo.AsQueryable()
                             .Where(f => f.CreateBy == createBy && f.IsDeleted == false)
                             .ToListAsync();
        }
        public async Task<int> InsertFactoryAsync(FactoryDto dto)
        {
            var entity = new Factory
            {
                FacId = dto.FacId ?? 0,
                FacName = dto.FacName,
                FacAddress = dto.FacAddress,
                FacShort = dto.ShortName,
                IsDeleted = dto.IsDeleted,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime ?? DateTime.Now
            };

            // Insert và trả về Id
            return (int)await Repo.InsertReturnIdentityAsync(entity);
        }
        // Overload that accepts numeric user id and converts to string
        public async Task<List<Factory>> GetListByCreateByAsync(long userId)
        {
            return await GetListByCreateByAsync(userId.ToString());
        }
        public async Task<bool> UpdateFactoryAsync(Factory factory)
        {
            return await Repo.UpdateAsync(factory) > 0;
        }
        public async Task<int> DeleteFactoryAsync(int facId)
        {
            return await Repo.DeleteAsync(f => f.FacId == facId);
        }
    }
}
