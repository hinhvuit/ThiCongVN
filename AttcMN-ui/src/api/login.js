import request from '@/utils/request'

// 登录方法
export function login(username, password, code, uuid) {
  const data = {
    username,
    password,
    code,
    uuid
  }
  return request({
    url: '/login',
    headers: {
      isToken: false,
      repeatSubmit: false
    },
    method: 'post',
    data: data
  })
}

// 注册方法
export function register(data) {
  return request({
    url: '/register',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}

// 获取用户详细信息
export function getInfo() {
  return request({
    url: '/getInfo',
    method: 'get'
  })
}

// 退出方法
export function logout() {
  return request({
    url: '/logout',
    method: 'post'
  })
}

// 获取验证码
export function getCodeImg() {
  return request({
    url: '/captchaImage',
    headers: {
      isToken: false
    },
    method: 'get',
    timeout: 20000
  })
}

// TODO: These APIs are not available in swagger, temporarily disabled
// 相信扫码
// export function xiangXinLogin(state) {
//   return request({
//     url: '/xiangxin/getXiangXinLogUrl?state=' + state,
//     method: 'get'
//   });
// }

// 验证是否为新用户
// export function checkIsNewUser() {
//   return request({
//     url: '/system/user/profile/getIsFirstLogin',
//     method: 'get'
//   });
// }

// 相信退出
// export function xiangXinLogout() {
//   return request({
//     url: '/system/user/profile/getXiangXinLoginOutUrl',
//     method: 'get'
//   });
// }