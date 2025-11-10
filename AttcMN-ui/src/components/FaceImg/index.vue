<template>
  <div>
    <div>
      <div class="flex mb_20">
        <div>
          <!-- 缩略图显示-->
          <div class="img-show" v-if="imgUrl">
            <img :src="imgUrl" class="avatar" ref="filesUp">
            <span class="actions">
              <!-- 放大 -->
              <span class="item" @click="enlarge()">
                <el-icon >
                  <ZoomIn />
                </el-icon>
                <!-- <i class="el-icon-zoom-in" @click="enlarge()"></i> -->
              </span>
              <!-- 删除 -->
              <span class="item" @click="del()">
                <el-icon>
                  <Delete />
                </el-icon>
                <!-- <i class="el-icon-delete" @click="del()"></i> -->
              </span>
            </span>
          </div>
          <!-- 图片上传 -->
          <el-upload v-loading.fullsreen.lock='loading' v-else action="#" class="uploader-avatar" list-type="picture"
            :auto-upload="false" :show-file-list="false" :on-change="imgPreview">
            <el-icon class="avatar-uploader-icon">
              <Plus />
            </el-icon>
            <!-- <i class="el-icon-plus avatar-uploader-icon"></i> -->
          </el-upload>
          <!-- 图片显示对话框 -->
          <el-dialog v-model="dialogVisible">
              <el-image style="width: 100%;" :src="dialogUrl" />
          </el-dialog>
        </div>
      </div>
    </div>
  </div>

</template>

<script>
import { uploadByBucket, toolgetKey,faceApi,getUriPrefixEnv } from '../../api/public/faceImg'
// import meta from 'import-meta';
export default {
  name: "faceImg",
  data() {
    return {
      loading: false,
      imgUrl: '',
      dialogVisible: false,
      dialogUrl: '',
      httpURl: '',
      url: '',
      apiUrl:{
        removeBackground:{
          type:'removeBackground',
          file:'',
          key:'',
          url:''
        },
        facialPhotoCropping:{
          type:'facialPhotoCropping',
          file:'',
          key:'',
          url:''
        },
        facialQualityInspection:{
          type:'facialQualityInspection',
          file:'',
          key:'',
          url:''
        },
        photoCompression:{
          type:'photoCompression',
          file:'',
          key:'',
          url:''
        },
      },
      apiPosturl:''
    }
  },
  props: {
    imgUrls: {
      type: String,
      default: ''
    },
    imgType: {
      type: String,
      default: ''
    },
    bucket: {
      type: String,
      default: ''
    },

  },
  created() {
    getUriPrefixEnv().then((res)=>{
      console.log(res);
      this.apiPosturl = res.data;
    })
    // console.log('this.$root.env',import.meta.env.VITE_APP_ENV);
    // let VITE_APP_ENV = import.meta.env.VITE_APP_ENV;
    //   if (VITE_APP_ENV === "production") {
    //     this.apiUrl.removeBackground.url = "http://10.132.164.112/api/v1/wcs/tools/removeBackground";
    //     this.apiUrl.facialPhotoCropping.url = "http://10.132.164.112/api/v1/wcs/tools/facialPhotoCropping";
    //     this.apiUrl.facialQualityInspection.url = "http://10.132.164.112/api/v1/wcs/tools/facialQualityInspection";
    //     this.apiUrl.photoCompression.url = "http://10.132.164.112/api/v1/wcs/tools/photoCompression";
    //   } else {
    //     this.apiUrl.removeBackground.url = "http://10.132.164.112:6600/api/ocr/ocrInvoke/removeBackground";
    //     this.apiUrl.facialPhotoCropping.url = "http://10.132.164.112:6600/api/ocr/ocrInvoke/facialPhotoCropping";
    //     this.apiUrl.facialQualityInspection.url = "http://10.132.164.112:6600/api/ocr/ocrInvoke/facialQualityInspection";
    //     this.apiUrl.photoCompression.url = "http://10.132.164.112:6600/api/ocr/ocrInvoke/photoCompression";
    //   }   

   },
  watch: {
    imgUrl(newVal, oldVal) {
      this.httpURl = newVal
      this.$emit('img', newVal)
    },
    imgUrls(newVal, oldVal) {
      this.imgUrl = newVal
    },
    immediate: true,
    deep: true
  },
  methods: {
    //图片缩略图
    imgPreview: function (file, fileList) {
      this.loading = true
      if (file.raw.type != "image/png" && file.raw.type != "image/jpeg" && file.raw.type != "image/jpg") {
        this.$message.error(this.$t('public.FaceImgTips2'));
        this.loading = false
        return false
      }
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isLt2M) {
        this.loading = false
        this.$message.error(this.$t('public.FaceImgTips1'));
        return false
      }
      //生成临时缩略图
      this.imgUrl = URL.createObjectURL(file.raw);
      let formData = new FormData()
      console.log('bucket',this.bucket);
      console.log('imgType',this.imgType);
      
      formData.append("file", file.raw)
      formData.append("bucket", this.bucket)
      this.imgUrl = ''
      // 上传图片为人脸
      if (this.imgType == 'header') {
        // this.apiUrl.removeBackground.key = this.getKeyInfo('removeBackground');
        // this.apiUrl.facialPhotoCropping.key = this.getKeyInfo('facialPhotoCropping');
        // this.apiUrl.facialQualityInspection.key = this.getKeyInfo('facialQualityInspection');
        // this.apiUrl.photoCompression.key = this.getKeyInfo('photoCompression');
        // this.fetchData();
        // let formData1 = new FormData();
        // formData1.append("invokeKey", this.apiUrl.facialQualityInspection.key)
        // formData1.append("faceFile1", file.raw)
        // faceApi(this.apiPosturl,'facialQualityInspection',formData1).then(res => {
        //   console.log('faceApi',res);
        // })


        // 获取人脸照片检测key
        toolgetKey({ type: 'facialQualityInspection' }).then((res) => {
          if (!res.data.invokeKey) {
            return this.$message.error(this.$t('public.keyWarning'));
          }
          this.apiUrl.facialQualityInspection.key = res.data.invokeKey;
          let formData1 = new FormData();
          formData1.append("invokeKey", this.apiUrl.facialQualityInspection.key)
          formData1.append("faceFile1", file.raw)
          // 人脸照片质量检测
          faceApi(this.apiPosturl, 'facialQualityInspection', formData1).then(res => {
            console.log('人脸照片质量检测');
            // 获取人脸裁剪key
            toolgetKey({ type: 'facialPhotoCropping' }).then((res) => {
              if (!res.data.invokeKey) {
                return this.$message.error(this.$t('public.keyWarning'));
              }
              this.apiUrl.facialPhotoCropping.key = res.data.invokeKey;
              let formData2 = new FormData();
              formData2.append("invokeKey", this.apiUrl.facialPhotoCropping.key)
              formData2.append("faceFile1", file.raw)
              // 人脸照片裁剪
              faceApi(this.apiPosturl, 'facialPhotoCropping', formData2).then(res => {
                console.log('人脸照片裁剪');
                // this.apiPosturl.removeBackground.file = res.imageResult;
                let formData3 = new FormData();
                formData3.append("faceFile1", this.dataURLtoBlob(res.imageResult))
                // 图片去除背景key
                  toolgetKey({ type: 'removeBackground' }).then((res) => {
                    if (!res.data.invokeKey) {
                      return this.$message.error(this.$t('public.keyWarning'));
                    }
                    this.apiUrl.removeBackground.key = res.data.invokeKey;
                    formData3.append("invokeKey", this.apiUrl.removeBackground.key)
                    // 图片去除背景
                    faceApi(this.apiPosturl, 'removeBackground', formData3).then(res => {
                      console.log('图片去除背景');
                      let formData4 = new FormData();
                      formData4.append("faceFile1", this.dataURLtoBlob(res.imageResult))
                      this.imgUrl = `data:image/jpeg;base64,${res.imageResult}`;
                      //图片压缩key
                      toolgetKey({ type: 'photoCompression' }).then((res) => {
                        if (!res.data.invokeKey) {
                          return this.$message.error(this.$t('public.keyWarning'));
                        }
                        this.apiUrl.photoCompression.key = res.data.invokeKey;
                        formData4.append("invokeKey", this.apiUrl.photoCompression.key)
                        // 图片压缩
                        faceApi(this.apiPosturl, 'photoCompression', formData4).then(res => {
                          console.log('图片压缩');
                          let formData5 = new FormData();
                          formData5.append("bucket", this.bucket)
                          formData5.append("file",this.dataURLtoBlob(res.imageResult) )
                          uploadByBucket(formData5).then(Response => {
                            console.log('uploadByBucket');
                            this.imgUrl = Response.data[0]
                          }).catch((res) => {
                            this.loading = false
                          });
                        }).catch((res) => {
                          this.loading = false
                        });
                      })
                    }).catch((res) => {
                      this.loading = false
                    });
                  });
              }).catch((res) => {
                this.loading = false
              });
            });
          }).catch((res) => {
            this.loading = false
          })
        });

      }else{
        // 非人脸图片
        uploadByBucket(formData).then(Response => {
          this.imgUrl = this.imgType == 'header' ? '' : Response.data[0]
        }).catch((res) => {
          this.loading = false
        })
      }
    },
    // Base64字符串转二进制流
    dataURLtoBlob(url) {
      let dataurl = `data:image/jpeg;base64,${url}`;
      let arr = dataurl.split(",");
      // console.log('arr',arr);
      let mime = arr[0].match(/:(.*?);/)[1];
      let bstr = atob(arr[1]);
      let n = bstr.length;
      let u8arr = new Uint8Array(n);
      while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
      }
      return new Blob([u8arr], {
        type: mime
      });
    },
    getKeyInfo(type) {
      toolgetKey({ type: type }).then((res) => {
        res.data.invokeKey ? res.data.invokeKey : ''
        if (!res.data.invokeKey) {
          this.$message.error(this.$t('public.keyWarning'));
        }
      });
    },
    enlarge: function () {
      
      this.loading = false
      this.dialogVisible = true;
      this.dialogUrl = this.imgUrl;
      console.log('this.dialogUrl',this.dialogUrl);
    },
    del: function () {
      this.loading = false
      this.imgUrl = '';
      this.dialogUrl = '';
    },
  },
  computed: {
    imgUrlsFun() {
      return this.imgUrl = this.imgUrls
    }
  }






}
</script>

<style lang='scss' scoped>
// .uploader-avatar{
//     width: 148px;
//     height: 148px;
//     border: 1px dashed #c0ccda;    
// }
// .uploader-avatar>>>.el-upload {
//     background-color: #fbfdff;
//     border: 1px dashed #c0ccda;
//     border-radius: 6px;
//     box-sizing: border-box;
//     width: 148px;
//     height: 148px;
//     cursor: pointer;
//     line-height: 146px;
//     vertical-align: top;
//     overflow: hidden;
// }
.img-show {
  position: relative;
  // border: 1px solid #c0ccda;
  border-radius: 6px;
  box-sizing: border-box;
  width: 100px;
  height: 100px;
  cursor: pointer;
  overflow: hidden;
}

.uploader-avatar {
  .el-upload {
    &:hover {
      border-color: #409EFF;
    }
  }

  i {
    font-size: 28px;
    color: #8c939d;
  }
}

.avatar {
  width: 100px;
  height: 100px;
  display: block;
}

.actions {
  position: absolute;
  width: 100%;
  height: 100%;
  line-height: 100px;
  left: 0;
  top: 0;
  cursor: default;
  text-align: center;
  color: #fff;
  opacity: 0;
  font-size: 20px;
  background-color: rgba(0, 0, 0, .5);
  transition: opacity .3s;
}

.actions:hover {
  opacity: 1;
}

.actions:hover span {
  display: inline-block;
}

.actions span {
  display: none;
  margin: 0 5px;
  cursor: pointer;
}

.avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}

.avatar-uploader .el-upload:hover {
  border-color: #409EFF;
}

.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 100px;
  height: 100px;
  line-height: 100px;
  text-align: center;
  border-radius: 6px;
  border: 1px dashed #d9d9d9;
}

.avatar {
  width: 100px;
  height: 100px;
  display: block;
}
</style>
