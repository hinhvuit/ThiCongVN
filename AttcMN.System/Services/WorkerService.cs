using AttcMN.Common.Interceptors;
using AttcMN.Framework.Exceptions;
using AttcMN.System.Repositories;
using AttcMN.System.Slave.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.System.Services
{
    public class WorkerService: BaseService<Worker,WorkerDto>,ITransient
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly WorkerRepository _workerRepository;
        public WorkerService(ILogger<WorkerService> logger,
            WorkerRepository workerRepository)
        {
            BaseRepo = workerRepository;
            _logger = logger;
            _workerRepository = workerRepository;
        }
        public async Task<Worker> GetAsync(long? workerId)
        {
            var entity = await base.FirstOrDefaultAsync(e => e.WorkerId == workerId);
            return entity;
        }
        public async Task<Worker> GetWorkerByEmpNoAsync(string empNo)
        {
            return await _workerRepository.GetWorkerByEmpNoAsync(empNo);
        }
        public async Task<Worker> GetWorkerByIdnoAsync(string idno)
        {
            return await _workerRepository.GetWorkerByIdnoAsync(idno);
        }
        public async Task<Worker> GetWorkerByIdAsync(long workId)
        {
            return await _workerRepository.GetWorkerByIdAsync(workId);
        }
        public async Task<List<Worker>> GetListByCreateByAsync(string createBy)
        {
            return await _workerRepository.GetListByCreateByAsync(createBy);
        }
        public async Task<List<Worker>> GetListByCreateByAsync(long userId)
        {
            return await _workerRepository.GetListByCreateByAsync(userId);
        }
        public int DeleteWorkerByFacID(int facid)
        {
            return _workerRepository.DeleteWorkerByFacID(facid);
        }
        public List<WorkerDto> GetWorkerList(WorkerDto dto)
        {
            return _workerRepository.GetWorkerList(dto);
        }
        public async Task<bool> InsertWorkerAsync(WorkerDto data)
        {
            data.EmpNo = await _workerRepository.GenerateNextEmpNoAsync();
            return await _workerRepository.InsertAsync(data);
        }
        public async Task<int> UpdateWorkerAsync(WorkerDto data)
        {
            return await _workerRepository.UpdateAsync(data);
        }
        public async Task<int> DeleteWorkerAsync(long[] workerIds)
        {
            return await _workerRepository.DeleteAsync(workerIds);
        }
        public async Task<int> DeleteWorkerAsync(long workerId)
        {
            return await _workerRepository.DeleteAsync(workerId);
        }
        public async Task<List<Worker>> GetWorkersbyFacIdandDeptIdAsync(int facId, int deptId)
        {
            return await _workerRepository.GetListByFacIdanDeptIdAsync(facId, deptId);
        }
        public virtual async Task<SqlSugarPagedList<Worker>> GetPagedUserListAsync(WorkerDto dto)
        {
            return await _workerRepository.GetPagedListAsync(dto);
        }
        public virtual async Task<List<Worker>> GetWorkerListAsync(WorkerDto dto)
        {
            return await _workerRepository.GetListAsync(dto);
        }
        public List<WorkerDto> ToDtos(List<Worker> entities)
        {
            var dtos = entities.Adapt<List<WorkerDto>>();
            return dtos;
        }
        public async Task<string> ImportDtosAsync(List<WorkerDto> dtos, bool isUpdateSupport, string operName)
        {
            if (dtos.IsEmpty())
            {
                throw new ServiceException("导入用户数据不能为空！");
            }
            int successNum = 0;
            int failureNum = 0;
            StringBuilder successMsg = new StringBuilder();
            StringBuilder failureMsg = new StringBuilder();
            foreach (var dto in dtos)
            {
                try
                {
                    var empNo = dto.EmpNo;
                    var idno = dto.Idno;
                    var existingUser = await _workerRepository.GetWorkerByEmpNoAsync(empNo);
                    if (existingUser == null)
                    {
                        await InsertWorkerAsync(dto);
                        successNum++;
                        successMsg.AppendLine($"员工编号 {empNo} 导入成功");
                    }
                    else if (isUpdateSupport)
                    {
                        dto.WorkerId = existingUser.WorkerId;
                        await UpdateWorkerAsync(dto);
                        successNum++;
                        successMsg.AppendLine($"员工编号 {empNo} 更新成功");
                    }
                    else
                    {
                        failureNum++;
                        failureMsg.AppendLine($"员工编号 {empNo} 已存在");
                    }
                }
                catch (Exception ex)
                {
                    failureNum++;
                    failureMsg.AppendLine($"员工编号 {dto.EmpNo} 导入失败：{ex.Message}");
                    _logger.LogError(ex, "导入用户失败");
                }
            }
            if (failureNum > 0)
            {
                failureMsg.Insert(0, "很抱歉，导入失败！共 " + failureNum + " 条数据格式不正确，错误如下：");
                throw new ServiceException(failureMsg.ToString());
            }
            else
            {
                successMsg.Insert(0, "恭喜您，数据已全部导入成功！共 " + successNum + " 条，数据如下：");
            }

            return successMsg.ToString();
        }
    }
}
