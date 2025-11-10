import request from '@/utils/request'
import qs from 'qs'

// 查询参数列表
export function companyList(companyName,companyType) {
    return request({
      url: `/api/company/company/list?companyName=${companyName}&companyType=${companyType}`,
      method: 'get',
      // params: {companyName}      
    })
  }