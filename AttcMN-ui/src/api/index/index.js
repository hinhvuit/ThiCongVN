import request from '@/utils/request'
import Qs from 'qs'
export function indexFile(query) {
    return request({
        url: '/indexFile',
        headers: {
            repeatSubmit: false
        },
        method: 'get',
        params: query
    })
}


export function addindexFile(data) {
    console.log(data);
    return request({
        url: '/indexFile',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
        },
        method: 'post',
        data: data
    })
}
export function delindexFile(id) {
    return request({
        url: '/indexFile/' + id,
        method: 'delete'
    })
}

export function updateindexFile(data) {
    return request({
        url: '/indexFile',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
        },
        method: 'put',
        data: data
    })
}