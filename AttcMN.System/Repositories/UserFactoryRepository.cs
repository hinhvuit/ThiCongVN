using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Repositories
{
    public class UserFactoryRepository : BaseRepository<UserFactory, UserFactoryDto>
    {
        public UserFactoryRepository(ISqlSugarRepository<UserFactory> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }


        public override ISugarQueryable<UserFactory> Queryable(UserFactoryDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.UserId > 0, (t) => t.UserId == dto.UserId)
                .WhereIF(dto.FacID > 0, (t) => t.FacId == dto.FacID)
            ;
        }

        public override ISugarQueryable<UserFactoryDto> DtoQueryable(UserFactoryDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.UserId > 0, (t) => t.UserId == dto.UserId)
                .WhereIF(dto.FacID > 0, (t) => t.FacId == dto.FacID)
                .Select((t) => new UserFactoryDto
                {
                    UserId = t.UserId,
                    FacID = t.FacId
                });
        }
        public int DeleteUserFactoryByUserId(long userId)
        {
            return Repo.Delete(r => r.UserId == userId);
        }

        public int DeleteUserFactory(List<long> userIds)
        {
            return Repo.Delete(r => userIds.Contains(r.UserId));
        }

        /// <summary>
        /// 按 角色ID 查询数量
        /// </summary>
        public async Task<int> CountUserFactoryByRoleIdAsync(int facid)
        {
            return await Repo.CountAsync(ur => ur.FacId == facid);
        }

        /// <summary>
        /// 按用户+角色删除
        /// </summary>
        public async Task<int> DeleteUserFactoryInfoAsync(int facId, long userId)
        {
            return await Repo.DeleteAsync(r => r.FacId == facId && r.UserId == userId);
        }
        public async Task<int> DeleteUserRoleInfoAsync(int facid, List<long> userIds)
        {
            return await Repo.DeleteAsync(r => r.FacId == facid && userIds.Contains(r.UserId));
        }
    }
}
