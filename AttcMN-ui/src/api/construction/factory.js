import request from '@/utils/request'

// 查询工厂列表
export function listFactory(query) {
  return request({
    url: '/system/factory/list',
    method: 'get',
    params: query
  })
}

// 查询工厂详细
export function getFactory(facId) {
  return request({
    url: '/system/factory/' + facId,
    method: 'get'
  })
}

// 新增工厂
export function addFactory(data) {
  return request({
    url: '/system/factory',
    method: 'post',
    data: data
  })
}

// 修改工厂
export function updateFactory(data) {
  return request({
    url: '/system/factory',
    method: 'put',
    data: data
  })
}

// 删除工厂
export function delFactory(facId) {
  return request({
    url: '/system/factory/' + facId,
    method: 'delete'
  })
}

// 查询所有工厂（下拉选项用）
export function getAllFactories() {
  return request({
    url: '/system/factory/all',
    method: 'get'
  })
}
