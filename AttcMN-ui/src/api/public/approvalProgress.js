import request from '@/utils/request'
import qs from 'qs'

// 查询参数列表
export function getSignProcessTemplate(businessKey) {
    return request({
      url: '/api/signToolApi/getSignProcessTemplate',
      method: 'get',
      params: {businessKey}      
    })
  }