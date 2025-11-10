<template>
  <div>
    <el-upload ref="uploadRef" action="#" list-type="picture-card" :on-change="handleChanges" :auto-upload="false"
      :limit="3" :class="{ hide: showUpload }"  v-model:file-list="fileList">
      <!-- <span class="file_Span">+</span> -->
      <el-icon><Plus /></el-icon>
      <template #file="{ file }">
        <div>
          <img  style="width: 100px; height: 100px" class="el-upload-list__item-thumbnail" :src="file.url" alt="">
          <span class="el-upload-list__item-actions">
            <span class="el-upload-list__item-preview" @click="handlePictureCardPreview(file)">
              <el-icon><zoom-in /></el-icon>
            </span>
            <span v-if="!disabled" class="el-upload-list__item-delete" @click="handleRemove(file, fileList)">
              <el-icon><Delete /></el-icon>
            </span>
          </span>
        </div>
      </template>
    </el-upload>
    <el-dialog v-model="dialogVisible">
      <img w-full style="width: 100%;" :src="dialogImageUrl" alt="Preview Image" />
      <!-- <img width="100%" :src="dialogImageUrl" alt=""> -->
    </el-dialog>
  </div>
</template>

<script setup name="goodsCheckDetails">
import { ref } from 'vue';
// import { uploadPicture } from '../../api/public/faceImg'
import { uploadByBucket } from '../../api/public/faceImg'
const { proxy } = getCurrentInstance();
const dialogImageUrl = ref('')
const dialogVisible = ref(false)
const disabled = ref(false)
const showUpload = ref(false)
const imgObject = ref({})
const fileList = ref([])
const files = ref([])
const typefile = ref("")
const uploadRef = ref(null)

const props = defineProps({
  fileInfo: {
    type: Array,
    default: []
  },
  bucket: {
    type: String,
    default: ''
  },
});
watch(
  () => props.fileInfo,
  (newVal, oldVal) => {
    files.value = newVal
  },
  { deep: true },
  { immediate: true },
);
const handleRemove = ((file, uploadFiles) => {
  console.log('file',file);
  console.log('uploadFiles',uploadFiles);
  // console.log('uploadRef',uploadRef);
  // console.log('uploadRef.value',uploadRef.value.uploadFiles);
  const index = uploadFiles.findIndex(e => e.uid === file.uid);
  // uploadFiles.splice(index, 1);
  fileList.value.splice(index, 1);
  proxy.$emit('zianImgs', fileList.value)
  showUpload.value = uploadFiles.length >= 3
})

const handlePictureCardPreview = ((file) => {
  console.log(file)
  if (typefile.value == "application/pdf") {
    for (let item of fileList.value) {
      const index = uploadRef.value.uploadFiles.findIndex(e => e.uid === file.uid);
      return window.open(fileList.value[index].url, "_blank")
    }
  }
  
  dialogImageUrl.value = file.url
  dialogVisible.value = true;
  console.log('dialogImageUrl',dialogImageUrl.value);
});

const handleChanges = ((file) => {
  console.log('handleChanges file', file);
  
  if (file.raw.type != "image/png" && file.raw.type != "image/jpeg" && file.raw.type != "image/jpg") {
    proxy.$message.error(proxy.$t('public.FaceImgTips2'));
    const index = uploadRef.value.uploadFiles.findIndex(e => e.uid === file.uid);
    uploadRef.value.uploadFiles.splice(index, 1);
    return false
  }
  const isLt2M = file.size / 1024 / 1024 < 2;
  if (!isLt2M) {
    proxy.$message.error(proxy.$t('public.FaceImgTips1'));
    const index = uploadRef.value.uploadFiles.findIndex(e => e.uid === file.uid);
    uploadRef.value.uploadFiles.splice(index, 1);
    return false
  }

  let formdata = new FormData();
  formdata.append("file", file.raw);
  formdata.append("bucket", props.bucket)
  uploadByBucket(formdata).then(val => {
    let imgUrl = ''
    typefile.value = file.raw.type
    if (typefile.value == "application/pdf") {
    } else {
      imgUrl = val.data[0]
    }
    console.log('val.data[0]',val.data[0]);
    fileList.value[fileList.value.length - 1].showUrl = val.data[0];
    showUpload.value = fileList.value.length >= 3
    // fileList.value.push({
    //   url: imgUrl,
    //   showUrl: val.data[0]

    // })
    proxy.$emit('zianImgs', fileList.value)
  })
  // showUpload.value = fileList.value.length >= 3
  
})


</script>

<style scoped>
 ::v-deep .el-upload-list__item ,.el-upload-list--picture-card .el-upload-list__item-actions{
  transition: none !important;
}

::v-deep .el-upload--picture-card{
  width:100px;
  height: 100px;
  line-height: 100px;  
  border: 1px dashed #c0ccda
}
::v-deep .el-upload-list--picture-card .el-upload-list__item{
  width:100px;
  height: 100px;
  /* line-height: 100px; */
  margin:0 8px 0 0;
  border: 1px dashed #c0ccda;
}
 ::v-deep .hide .el-upload--picture-card {
  display: none !important;
}
</style>
