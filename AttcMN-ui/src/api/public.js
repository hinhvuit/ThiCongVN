import request from '@/utils/request';
import axios from 'axios';
export function openFile(url) {
  axios
    .get(url, {
      responseType: 'blob'
    })
    .then((res) => {
      window.open(URL.createObjectURL(new Blob([res.data], { type: 'application/pdf' })));
    })
    .catch((err) => {
      console.log(err);
    });
}
// 获取所有园区
export const getAllPark = () => {
  return request({
    url: '/Business/getParkList',
    method: 'get'
  });
};

// 上传文件
export function uploadFile(data) {
  return request({
    url: '/uploadFile/upload',
    method: 'post',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
    },
    data
  });
}

// 根据园区id获取园区管理员
export function getParkAdmin(id) {
  return request({
    url: '/boss/getBossByPark/' + id,
    method: 'get'
  });
}
