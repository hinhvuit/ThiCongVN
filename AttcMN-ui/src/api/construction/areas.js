import request from '@/utils/request'

// 获取施工区域列表
export function listAreas(query) {
  return request({
    url: '/construction/areas/list',
    method: 'get',
    params: query
  })
}

// 获取施工区域详细信息
export function getArea(id) {
  return request({
    url: '/construction/areas/' + id,
    method: 'get'
  })
}

// 新增施工区域
export function addArea(data) {
  return request({
    url: '/construction/areas',
    method: 'post',
    data: data
  })
}

// 修改施工区域
export function updateArea(data) {
  return request({
    url: '/construction/areas',
    method: 'put',
    data: data
  })
}

// 删除施工区域
export function delArea(id) {
  return request({
    url: '/construction/areas/' + id,
    method: 'delete'
  })
}

// 获取所有施工区域（用于下拉选择）
export function getAllAreas() {
  return request({
    url: '/construction/areas/all',
    method: 'get'
  })
}

// 获取当前用户有权限的施工区域
export function getUserAreas() {
  return request({
    url: '/construction/areas/user',
    method: 'get'
  })
}
