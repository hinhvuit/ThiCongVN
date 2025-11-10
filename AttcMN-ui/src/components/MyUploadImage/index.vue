<!-- 
  @FileDescription: 图片上传组件
  @params:{
    url: 图片地址
    fileSizeMax: 文件最大限制
    fileSizeMin: 文件最小限制
    fileType: 文件类型, 例如['png', 'jpg', 'jpeg']
    isShowTip: 是否显示提示
  }
  @methods:{
    uploadChange:( file )=>{} 图片上传回调
  }
  @Date:2024-03-19 14:10:05
  @Author:LZW
-->
<template>
  <div>
    <input type="file" ref="imageUpload" @change="imageChange" v-show="false" />
    <div v-if="imageUrl" class="image-box">
      <div class="image-mask">
        <div class="icon-box">
          <div @click="showImageView">
            <el-icon><View /></el-icon>
          </div>
          <div @click="deleteImage">
            <el-icon><Delete /></el-icon>
          </div>
        </div>
      </div>
      <el-image class="image-e" style="width: 100px; height: 100px" :src="imageUrl" fit="contain"></el-image>
    </div>
    <div v-else class="image-uploader" @click="inputClick">
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
    </div>
    <!-- el-image 上自带的图片预览小组件 可以直接调用 -->
    <el-image-viewer
      @close="
        () => {
          dialogVisible = false;
        }
      "
      v-if="dialogVisible"
      :url-list="[imageUrl]"
    />
  </div>
</template>

<script setup>
  import { ElMessage } from 'element-plus';
  const { proxy } = getCurrentInstance();
  const emit = defineEmits();

  const myFile = ref('');
  const imageUrl = ref('');
  const dialogVisible = ref(false);
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
      default: 100
    },
    fileSizeMin: {
      type: Number,
      default: 50
    },
    // 文件类型, 例如['png', 'jpg', 'jpeg']
    fileType: {
      type: Array,
      default: () => ['png', 'jpg', 'jpeg']
    },
    // 是否显示提示
    isShowTip: {
      type: Boolean,
      default: true
    }
  });
  onMounted(() => {
    imageUrl.value = props.url;
  });
  watch(
    () => props.url,
    (newValue) => {
      imageUrl.value = newValue;
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
    proxy.$refs.imageUpload.click();
  }
  function imageChange(e) {
    const file = e.target.files[0];
    const imgSize = parseInt(file.size / 1024);
    let isImg = false;
    // let fileType = file.name.split('.')[1]
    // isImg = props.fileType.includes(fileType)
    if (props.fileType.length) {
      let fileExtension = '';
      if (file.name.lastIndexOf('.') > -1) {
        fileExtension = file.name.slice(file.name.lastIndexOf('.') + 1);
      }
      isImg = props.fileType.some((type) => {
        if (file.type.indexOf(type) > -1) return true;
        if (fileExtension && fileExtension.indexOf(type) > -1) return true;
        return false;
      });
    } else {
      isImg = file.type.indexOf('image') > -1;
    }
    if (!isImg) {
      proxy.$refs.imageUpload.value = null;
      return proxy.$modal.msgError(proxy.$t('public.Theformatshouldbe')+`${props.fileType.join('/')}`);
    }
    if (imgSize < props.fileSizeMin || imgSize > props.fileSizeMax) {
      proxy.$refs.imageUpload.value = null;
      return ElMessage({
        dangerouslyUseHTMLString: true,
        type: 'error',
        message: `<span>${sizeText.value}</span>`
      });
    }
    myFile.value = file;
    imageUrl.value = URL.createObjectURL(file);
    emit('uploadChange', myFile.value);
    proxy.$refs.imageUpload.value = null;
  }
  function showImageView() {
    dialogVisible.value = true;
  }
  function deleteImage() {
    myFile.value = '';
    imageUrl.value = '';
    emit('uploadChange', myFile.value);
  }
</script>

<style scoped>
  /* @import url(); 引入公共css类 */
  .image-uploader {
    width: 100px;
    height: 100px;
    border: 1px dashed #d9d9d9;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
  }
  .image-uploader:hover {
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
  .image-box {
    width: 100px;
    height: 100px;
    display: block;
    overflow: hidden;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
  }
  .image-box .image-e {
    z-index: 0;
  }
  .image-mask {
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
  .image-box:hover .image-mask {
    opacity: 1;
  }
  .tip {
    font-size: 12px;
  }
</style>
