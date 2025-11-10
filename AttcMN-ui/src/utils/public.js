//判断身份证格式
export function isIdCard(val){
    let reg = /^[1-9][0-9]{5}([1][9][0-9]{2}|[2][0][0|1][0-9])([0][1-9]|[1][0|1|2])([0][1-9]|[1|2][0-9]|[3][0|1])[0-9]{3}([0-9]|[Xx])$/;
    return reg.test(val);
}

//判断邮箱格式
export function isEmail(str) { 
    if (!/^[\d\w\s-./]+[@][\d\w-./]+$/.test(str))  
      return true
    return false
  }

//判断手机号格式
export function IsPhone(str) {
  if (!/^1\d{10}$/.test(str))
    return true
  return false
}

//正整数
export function isNum(str) {
  let reg = /^[1-9]\d*$/;
  return reg.test(str);
}

//判断是否为空
export function isNull(val) {
  if(val==undefined||val==null||val==''){
    return true
  }
  return false
}

//时间戳转换为年月日
export function changeDateString(val){
   let time = new Date(val);
   let YY = time.getFullYear();
   let MM = time.getMonth() + 1 < 10 ? '0' + (time.getMonth()+1) : (time.getMonth()+1);
   let DD = time.getDate() < 10 ? '0' + time.getDate() : time.getDate();
   return YY + '-' + MM + '-' + DD;
};
//年月日时分秒
export function changeDateTimeString(val){
  let time = new Date(val)
   let YY = time.getFullYear();
   let MM = time.getMonth() + 1 < 10 ? '0' + (time.getMonth()+1) : (time.getMonth()+1);
   let DD = time.getDate() < 10 ? '0' + time.getDate() : time.getDate();
   let hh = time.getHours() < 10 ? '0' + time.getHours() : time.getHours(); 
   let mm = time.getMinutes() < 10 ? '0' + time.getMinutes() : time.getMinutes(); 
   let ss = time.getSeconds() < 10 ? '0' + time.getSeconds() : time.getSeconds();
   return  YY + '-' + MM + '-' + DD + ' ' + hh + ':' + mm + ':' + ss;
};
//时分秒
export function changeTimeString(val){
  let time = new Date(val)
   let hh = time.getHours() < 10 ? '0' + time.getHours() : time.getHours(); 
   let mm = time.getMinutes() < 10 ? '0' + time.getMinutes() : time.getMinutes(); 
   let ss = time.getSeconds() < 10 ? '0' + time.getSeconds() : time.getSeconds();
   return  hh + ':' + mm + ':' + ss;
  };


//车牌
export function licensePlateNum(value) {
  const plateNumber = /^([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}(([0-9]{5}[DF])|([DF]([A-HJ-NP-Z0-9])[0-9]{4})))|([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳]{1})$/
  if(!plateNumber.test(value)) return false
  return true
}

//证件*号处理
export function butyNum(value,btnShow) {
  if(value==''||value==undefined||value==null|| value.length <= 6||btnShow==true){
    return value
  }
  // 身份证号
  if (value.length == 18) {
    return value.replace(/^(.{4})(?:\d+)(.{4})$/, "\$1******\$2")
  }
  if (value.length == 15) {
    return value.replace(/^(.{3})(?:\d+)(.{3})$/, "\$1*****\$2")
  }
  // 其他大于六位
  else{
    let str=value.substring(0,2) + value.substring(2,value.length - 2).replace(/./g, '*')+ value.substring(value.length - 2)
    return str
  }
}
