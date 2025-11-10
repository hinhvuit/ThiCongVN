import request from '@/utils/request'

// 获取施工人员列表
export function listWorkers(query) {
  return request({
    url: '/construction/workers/list',
    method: 'get',
    params: query
  })
}

// 获取施工人员详细信息
export function getWorker(id) {
  return request({
    url: '/construction/workers/' + id,
    method: 'get'
  })
}

// 新增施工人员
export function addWorker(data) {
  return request({
    url: '/construction/workers',
    method: 'post',
    data: data
  })
}

// 修改施工人员
export function updateWorker(data) {
  return request({
    url: '/construction/workers',
    method: 'put',
    data: data
  })
}

// 删除施工人员
export function delWorker(id) {
  return request({
    url: '/construction/workers/' + id,
    method: 'delete'
  })
}

// 批量删除施工人员
export function delWorkers(ids) {
  return request({
    url: '/construction/workers/' + ids,
    method: 'delete'
  })
}

// 导出施工人员
export function exportWorkers(query) {
  return request({
    url: '/construction/workers/export',
    method: 'get',
    params: query
  })
}

// 导入施工人员
export function importWorkers(data) {
  return request({
    url: '/construction/workers/import',
    method: 'post',
    data: data
  })
}

// 获取施工人员模板
export function downloadTemplate() {
  return request({
    url: '/construction/workers/template',
    method: 'get'
  })
}

// 获取过期证书列表
export function getExpiredCertificates(query) {
  return request({
    url: '/construction/workers/expired-certificates',
    method: 'get',
    params: query
  })
}

// 按承包商统计
export function getWorkersByContractor() {
  return request({
    url: '/construction/workers/stats/contractor',
    method: 'get'
  })
}

// 按区域统计
export function getWorkersByArea() {
  return request({
    url: '/construction/workers/stats/area',
    method: 'get'
  })
}
