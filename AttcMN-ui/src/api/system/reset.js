import request from '@/utils/request';
import Qs from 'qs';

// 获取前台用户
export function getFrontUser(data) {
  return request({
    url: '/userInfo/getQTUserInfo',
    method: 'get',
    params: data
  });
}

// 获取后台用户
export function getBackUser(data) {
  return request({
    url: '/userInfo/getHTUserInfo',
    method: 'get',
    params: data
  });
}

// 重置密码
export function resetPassword(data) {
  return request({
    url: '/userInfo/ResetPassword',
    method: 'post',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
    },
    data: Qs.stringify(data)
  });
}
