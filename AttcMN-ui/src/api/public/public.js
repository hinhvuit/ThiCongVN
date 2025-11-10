import request from '@/utils/request'
import qs from 'qs'

//发送验证验证码
export function phoneNum(phoneNum,CodeType) {
    const data={phoneNum,CodeType}
    return request({
      url: '/dev-api/SMS/phoneNum',
      method: 'post',
      headers: {
        "Content-Type": "application/x-www-form-urlencoded"
      },
      data: qs.stringify(data)     
    })
  }

//验证手机号
export function validatorCode(phone,code) {
    const data={phone,code}
    return request({
      url: '/dev-api/api/personnel/register/validatorCode',
      method: 'post',
      headers: {
        "Content-Type": "application/x-www-form-urlencoded"
      },
      data: qs.stringify(data)     
    })
  }  

// 重置密码
export function updatePwd(oldPassword,newPassword) {
  const data={oldPassword,newPassword}
  return request({
    url: '/dev-api/system/user/profile/updatePwd',
    method: 'put',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },    
    data: qs.stringify(data)   
  })
}  

// 重置密码
export function update(data) {
  return request({
    url: '/dev-api/api/business/personnel/update',
    method: 'post',    
    data
  })
} 
// 人员信息
export function PeopleInfo(number) {
  return request({
    url: `/PeopleInfo/${number}`,
    method: 'get',
  });
}