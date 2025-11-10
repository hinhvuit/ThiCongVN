import request from '@/utils/request'
import qs from 'qs'

// 查询签核模板
export function getTemplate(templateId) {
    return request({
      url: '/dev-api/api/template/template/getTemplate',
      method: 'get',
      params: {templateId}      
    })
  }

// 查询签核模板
export function checkPower(employNo,power) {
  const data={employNo,power}
  return request({
    url: '/dev-api/api/personel/personel/checkPower',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)     
  })
}

//查询审核人/核准人信息
export function selectPersonByEmployNo(employNo,personType) {
  const data={employNo,personType}
  return request({
    url: '/dev-api/api/personel/personel/selectPersonByEmployNo',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}

//查询审核人/核准人信息
export function querySignPerson(powerType) {
  const data={powerType}
  return request({
    url: '/dev-api/api/cache/querySignPerson',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}