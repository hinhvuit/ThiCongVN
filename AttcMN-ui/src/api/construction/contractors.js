import request from '@/utils/request'

// 获取承包商列表
export function listContractors(query) {
  return request({
    url: '/construction/contractors/list',
    method: 'get',
    params: query
  })
}

// 获取承包商详细信息
export function getContractor(id) {
  return request({
    url: '/construction/contractors/' + id,
    method: 'get'
  })
}

// 新增承包商
export function addContractor(data) {
  return request({
    url: '/construction/contractors',
    method: 'post',
    data: data
  })
}

// 修改承包商
export function updateContractor(data) {
  return request({
    url: '/construction/contractors',
    method: 'put',
    data: data
  })
}

// 删除承包商
export function delContractor(id) {
  return request({
    url: '/construction/contractors/' + id,
    method: 'delete'
  })
}

// 获取所有承包商（用于下拉选择）
export function getAllContractors() {
  return request({
    url: '/construction/contractors/all',
    method: 'get'
  })
}
