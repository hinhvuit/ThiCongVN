using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Repositories
{
    public class WorkerRepository : BaseRepository<Worker, WorkerDto>
    {
        public WorkerRepository(ISqlSugarRepository<Worker> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }
        public override ISugarQueryable<WorkerDto> DtoQueryable(WorkerDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.FacId > 0, (t) => t.FacId == dto.FacId && t.IsDeleted == false)
                .Select((t) => new WorkerDto
                {
                    EmpNo = t.EmpNo
                });
        }

        public override ISugarQueryable<Worker> Queryable(WorkerDto dto)
        {
            return Repo.AsQueryable()
                .LeftJoin<Factory>((t, d) => t.FacId == d.FacId)
                .WhereIF(dto.FacId > 0, (t) => t.FacId == dto.FacId)
                .WhereIF(dto.DeptId > 0, (t) => t.DeptId == dto.DeptId)
                .WhereIF(!string.IsNullOrEmpty(dto.EmpName),(t)=> t.EmpName!.Contains(dto.EmpName!))
                .WhereIF(!string.IsNullOrEmpty(dto.EmpNo), (t) => t.EmpNo!.Contains(dto.EmpNo!))
                .WhereIF(!string.IsNullOrEmpty(dto.Idno), (t) => t.Idno!.Contains(dto.Idno!))
                .WhereIF(dto.Params.BeginTime != null, (t) => t.CreateTime >= dto.Params.BeginTime)
                .WhereIF(dto.Params.EndTime != null, (t) => t.CreateTime <= dto.Params.EndTime)
                .Select((t, d) => t);
        }

        /// <summary>
        /// Tạo EmpNo tự động theo định dạng TC + 2 số cuối năm tiếp theo + số thứ tự 6 chữ số
        /// Ví dụ: TC26000001, TC26000002, ..., TC27000001
        /// </summary>
        /// <returns>EmpNo mới</returns>
        public async Task<string> GenerateNextEmpNoAsync()
        {
            // Lấy năm hiện tại + 1
            var nextYear = DateTime.Now.Year;
            var yearSuffix = (nextYear % 100).ToString("D2"); // Lấy 2 số cuối
            var prefix = $"TC{yearSuffix}"; // Ví dụ: TC26

            // Tìm EmpNo lớn nhất có cùng prefix
            var maxEmpNo = await Repo.AsQueryable()
                .Where(w => w.EmpNo.StartsWith(prefix))
                .OrderByDescending(w => w.EmpNo)
                .Select(w => w.EmpNo)
                .FirstAsync();

            int nextSequence = 1;

            if (!string.IsNullOrEmpty(maxEmpNo) && maxEmpNo.Length == 10)
            {
                // Lấy 6 số cuối và tăng lên 1
                var sequencePart = maxEmpNo.Substring(4); // Bỏ "TC26"
                if (int.TryParse(sequencePart, out int currentSequence))
                {
                    nextSequence = currentSequence + 1;
                }
            }

            // Tạo EmpNo mới: TC + YY + XXXXXX (6 chữ số)
            return $"{prefix}{nextSequence:D6}";
        }

        public int DeleteWorkerByFacID(int facid)
        {
            return Repo.Delete(r => r.FacId == facid);
        }
        public List<WorkerDto> GetWorkerList(WorkerDto dto)
        {
            return DtoQueryable(dto).ToList();
        }
        public async Task<Worker> GetWorkerByEmpNoAsync(string empNo)
        {
            return await this.FirstOrDefaultAsync(f => f.EmpNo == empNo && f.IsDeleted == false);
        }
        public async Task<Worker> GetWorkerByIdnoAsync(string idno)
        {
            return await this.FirstOrDefaultAsync(f => f.Idno == idno && f.IsDeleted == false);
        }
        public async Task<Worker> GetWorkerByIdAsync(long workId)
        {
            return await this.FirstOrDefaultAsync(f => f.WorkerId == workId && f.IsDeleted == false);
        }
        public async Task<List<Worker>> GetListByCreateByAsync(string createBy)
        {
            if (string.IsNullOrWhiteSpace(createBy))
                return new List<Worker>();
            return await Repo.AsQueryable()
                             .Where(f => f.CreateBy == createBy && f.IsDeleted == false)
                             .ToListAsync();
        }
        public async Task<List<Worker>> GetListByFacIdanDeptIdAsync(int facid,int depid)
        {
            return await Repo.AsQueryable()
                             .Where(f => f.FacId == facid && f.IsDeleted == false&&f.DeptId==depid)
                             .ToListAsync();
        }
        public async Task<List<Worker>> GetListByCreateByAsync(long userId)
        {
            return await GetListByCreateByAsync(userId.ToString());
        }
        public async Task<int> CountWorkerByDeptIdAsync(int deptId)
        {
            return await Repo.CountAsync(w => w.DeptId == deptId && w.IsDeleted == false);
        }
        public async Task<string> GetMaxEmpNo(int facid)
        {
            var worker = await Repo.AsQueryable()
                                 .Where(w => w.FacId == facid && w.IsDeleted == false)
                                 .OrderByDescending(w => w.EmpNo)
                                 .Select(w => w.EmpNo)
                                 .FirstAsync();
            return worker;
        }
    }
}
