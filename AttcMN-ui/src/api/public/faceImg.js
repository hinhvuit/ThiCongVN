import request from '@/utils/request'
import qs from 'qs'

// 图片链接转换
export function uploadPicture(file) {
    return request({
      url: '/api/visitorPreventEpidemic/visitorPreventEpidemic/uploadPicture',
      method: 'post',
      data: file, 
         
    })
  }


// 图片链接转换
export function uploadByBucket(file) {
    return request({
      url: '/uploadFileByMinio/uploadByBucket',
      method: 'post',
      data: file, 
         
    })
  }


// 图片去除背景
export function xinImageMattingPc(imageUrl) {
  const data={imageUrl}
  return request({
    url: '/weixin/file/xinImageMattingPc',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}

//身份证图片裁剪
export function xinCardCropPc(imageUrl,side) {
  const data={imageUrl,side}
  return request({
    url: '/img-api/weixin/file/xinCardCropPc',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}

//身份证图片ocr
export function xinCardOcrPc(imageUrl,side,cardType) {
  const data={imageUrl,side,cardType}
  return request({
    url: '/img-api/weixin/file/xinCardOcrPc',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}

//人脸图片裁剪
export function xinFaceCropPc(imageUrl) {
  const data={imageUrl}
  return request({
    url: '/weixin/file/xinFaceCropPc',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}

//图片质量检测
export function xinFaceQualityDetectPc(imageUrl) {
  const data={imageUrl}
  return request({
    url: '/weixin/file/xinFaceQualityDetectPc',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}

//图片压缩
export function xinIKARResizePc(imageUrl,size,saveImg) {
  const data={imageUrl,size,saveImg}
  return request({
    url: '/weixin/file/xinIKARResizePc',
    method: 'post',
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    },
    data: qs.stringify(data)
  })
}

export function toolgetKey(query) {
  return request({
    url: '/api/ocr/ocrOperateKey/getKeyInfo',
    method: 'get',
    params: query
  });
}

//工具箱人脸识别
export function faceApi(url,name,file) {
  return request({
    url: `${url}/${name}`,
    method: 'post',
    data: file
  })
}

//工具箱地址
export function getUriPrefixEnv() {
  return request({
    url: '/api/ocr/ocrInvoke/getUriPrefixEnv',
    method: 'get',
  })
}

