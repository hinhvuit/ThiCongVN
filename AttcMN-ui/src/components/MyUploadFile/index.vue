<!-- 
  @FileDescription:二进制文件上传组件
  @params:{}
  @Date:2024-03-19 14:14:33
  @Author:LZW
-->
<template>
  <div>
    <input type="file" ref="fileUpload" @change="fileChange" v-show="false" />
    <div v-if="fileUrl" class="file-box">
      <div class="file-mask">
        <div class="icon-box">
          <div @click="showFileView" v-if="myType=='pdf'">
            <el-icon><View /></el-icon>
          </div>
          <div @click="deleteFile">
            <el-icon><Delete /></el-icon>
          </div>
        </div>
      </div>
      <el-image class="file-e" style="width: 100px; height: 100px" :src="pdf" fit="contain"></el-image>
      <!-- <div class="btn-box">
        附件
        <el-button type="primary" link style="margin: 0 10px" @click="showFileView"> 预览 </el-button>
        <el-button type="danger" link @click="deleteFile"> 删除 </el-button>
      </div> -->
    </div>
    <div v-else class="file-uploader" @click="inputClick">
      <el-icon class="icon"><Plus /></el-icon>
    </div>

    <!-- 上传提示 -->
    <div class="tip" slot="tip" v-if="isShowTip">
      <!-- 请上传 -->
      <span v-if="myTip" v-html="myTip"></span>
      <span v-html="sizeText"></span>
      <span>
        {{$t('public.Theformatshouldbe')}} <b style="color: #f56c6c">{{ fileType.join('/') }}</b>
      </span>
      <!-- 的文件 -->
    </div>
  </div>
</template>

<script setup>
  import { ElMessage } from 'element-plus';
  import pdf from '@/assets/images/pdf.jpg';
  import { onMounted } from 'vue';

  const { proxy } = getCurrentInstance();
  const emit = defineEmits();

  const myFile = ref('');
  const fileUrl = ref('');
  const myType = ref('');
  const props = defineProps({
    url: {
      type: String
    },
    myTip: {
      type: String
    },
    // 大小限制(kB)
    fileSizeMax: {
      type: Number,
      default: 1024 * 5
    },
    fileSizeMin: {
      type: Number,
      default: 50
    },
    // 文件类型, 例如['png', 'jpg', 'jpeg']
    fileType: {
      type: Array,
      default: () => ['doc', 'xls', 'ppt', 'txt', 'pdf']
    },
    // 是否显示提示
    isShowTip: {
      type: Boolean,
      default: true
    }
  });
  onMounted(() => {
    fileUrl.value = props.url;
    // myFile.value = props.file;
  });
  watch(
    () => props.url,
    (newValue) => {
      fileUrl.value = newValue;
    }
  );
  const sizeText = computed(() => {
    let text = '';
    let min = props.fileSizeMin >= 1024 ? `${Math.floor(props.fileSizeMin / 1024)}MB` : `${props.fileSizeMin}KB`;
    let max = props.fileSizeMax >= 1024 ? `${Math.floor(props.fileSizeMax / 1024)}MB` : `${props.fileSizeMax}KB`;
    if (min == 0 && max > 0) {
      if(localStorage.locale == 'zh'){
        text = `大小在 <b style="color: #f56c6c">${min} - ${max}</b> 之间`;
      }
      if(localStorage.locale == 'en'){
        text = ` The size is <b style="color: #f56c6c">${min} - ${max}</b>`;
      }
    } else {
      if(localStorage.locale == 'zh'){
        text = `大小在 <b style="color: #f56c6c"> ${max} </b>以内`;
      }
      if(localStorage.locale == 'en'){
        text = ` The size is under  <b style="color: #f56c6c"> ${max} </b>`;
      }
    }
    return text;
  });
  function inputClick() {
    proxy.$refs.fileUpload.click();
  }
  function fileChange(e) {
    const file = e.target.files[0];
    const fileSize = parseInt(file.size / 1024);
    let isFile = false;
    if (props.fileType.length) {
      let fileExtension = '';
      if (file.name.lastIndexOf('.') > -1) {
        fileExtension = file.name.slice(file.name.lastIndexOf('.') + 1);
        myType.value = fileExtension;
      }
      isFile = props.fileType.some((type) => {
        if (file.type.indexOf(type) > -1) return true;
        if (fileExtension && fileExtension.indexOf(type) > -1) return true;
        return false;
      });
    } else {
      isFile = file.type.indexOf('file') > -1;
    }
    if (!isFile) {
      proxy.$refs.fileUpload.value = null;
      return proxy.$modal.msgError( proxy.$t('public.Theformatshouldbe') +`${props.fileType.join('/')}`);
    }
    if (fileSize < props.fileSizeMin || fileSize > props.fileSizeMax) {
      proxy.$refs.fileUpload.value = null;
      return ElMessage({
        dangerouslyUseHTMLString: true,
        type: 'error',
        message: `<span>${sizeText.value}</span>`
      });
    }
    myFile.value = file;
    fileUrl.value = window.URL.createObjectURL(new Blob([myFile.value], { type: 'application/pdf' }));
    emit('uploadChange', myFile.value);
    proxy.$refs.fileUpload.value = null;
  }
  function showFileView() {
    if (myFile.value) {
      let url = window.URL.createObjectURL(new Blob([myFile.value], { type: 'application/pdf' })); //获得一个pdf的url对象
      window.open(url, '_blank'); //打开一个新窗口
      URL.revokeObjectURL(url); //释放内存
    } else {
      window.open(props.url, '_blank');
    }
  }
  function deleteFile() {
    myFile.value = '';
    fileUrl.value = '';
    emit('uploadChange', myFile.value);
  }

  // const image = ref('');
  // const imageFunc = (url) =>
  //   import(url).then((res) => {
  //     image.value = res.default;
  //   });
  // // 使用
  // imageFunc('../../assets/images/pdf.jpg');
</script>

<style scoped>
  /* @import url(); 引入公共css类 */
  .file-uploader {
    width: 100px;
    height: 100px;
    border: 1px dashed #d9d9d9;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
  }
  .file-uploader:hover {
    border-color: #2c7be5;
  }
  .icon {
    font-size: 48px;
    color: #8c939d;
    width: 100px;
    height: 100px;
    line-height: 100px;
    text-align: center;
  }
  .icon:hover {
    color: #2c7be5;
  }
  .file-box {
    width: 100px;
    height: 100px;
    display: block;
    overflow: hidden;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
  }
  .file-box .file-e {
    z-index: 0;
  }
  .file-mask {
    position: absolute;
    top: 0;
    left: 0;
    width: 100px;
    height: 100px;
    background: rgba(101, 101, 101, 0.3);
    color: #ffffff;
    opacity: 0;
    z-index: 999;
  }
  .icon-box {
    width: 100px;
    height: 100px;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 20px;
  }
  .icon-box div {
    padding: 10px;
  }
  .file-box:hover .file-mask {
    opacity: 1;
  }
  .tip {
    font-size: 12px;
  }
</style>
