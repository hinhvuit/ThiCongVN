import request from '@/utils/request'

// 查询文档列表
export function listDocument(query) {
  return request({
    url: '/records/document/list',
    method: 'get',
    params: query
  })
}

// 查询文档详细
export function getDocument(docId) {
  return request({
    url: '/records/document/' + docId,
    method: 'get'
  })
}

// 新增文档
export function addDocument(data) {
  return request({
    url: '/records/document',
    method: 'post',
    data: data
  })
}

// 修改文档
export function updateDocument(data) {
  return request({
    url: '/records/document',
    method: 'put',
    data: data
  })
}

// 删除文档
export function delDocument(docId) {
  return request({
    url: '/records/document/' + docId,
    method: 'delete'
  })
}

// 根据工厂ID查询文档
export function getDocumentsByFactory(facId) {
  return request({
    url: '/records/document/factory/' + facId,
    method: 'get'
  })
}

// 查询文档类型列表
export function listDocumentType(query) {
  return request({
    url: '/records/document/type/list',
    method: 'get',
    params: query
  })
}

// 查询文档类型下拉列表
export function getDocumentTypes() {
  return request({
    url: '/records/document/types',
    method: 'get'
  })
}

// 导出文档
export function exportDocument(query) {
  return request({
    url: '/records/document/export',
    method: 'get',
    params: query
  })
}
