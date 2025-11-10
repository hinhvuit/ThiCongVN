<template>
  <div>
      <div>
          <div class="flex mb_20">
              <div>
                <!-- 缩略图显示-->
                <div class="img-show" v-if="allTypeUrl.imgUrl">
                  <!-- <img src="../../assets/img/PDF.jpg" class="avatar" ref="filesUp"> -->
                  <div v-if="allTypeUrl.pdfUrl">
                    <img src="../../assets/img/PDF.jpg" class="avatar" ref="filesUp">
                  </div>
                  <div v-else>
                    <!-- <img src="../../assets/img/PDF.jpg" class="avatar" ref="filesUp"> -->
                    <img :src="allTypeUrl.imgUrl"  class="avatar" ref="filesUp">
                  </div>
                	<span class="actions">
                		<!-- 放大 -->
                		<span class="item">
                			<i class="el-icon-zoom-in" @click="enlarge()"></i>
                		</span>
                		<!-- 删除 -->
                		<span class="item">
                			<i class="el-icon-delete" @click="del()"></i>
                		</span>
                	</span>
                </div>


                <!-- 图片上传 -->
                <el-upload 
                    v-loading.fullsreen.lock='loading'
                    v-else
                    action="#"
                    class="uploader-avatar" 
                    list-type="picture"
                    :auto-upload="false" 
                    :show-file-list="false"
                    :on-change="imgPreview">
                    	<i class="el-icon-plus avatar-uploader-icon"></i>
                    </el-upload>
                <!-- 图片显示对话框 -->
                <el-dialog :visible.sync="dialogVisible">
                	<img width="100%" :src="dialogUrl" alt="">
                </el-dialog>

              </div>
              </div>
          </div>
      </div>
 
</template>

<script>
import {uploadPicture,xinImageMattingPc,xinCardCropPc,xinCardOcrPc} from '../../api/public/faceImg'
export default {
    name:"uploadFile",
    data() {
        return {
            loading:false,
            imgUrl:'',
            dialogVisible: false,
            dialogUrl: '',
            httpURl:'',
            url:'',
            typefile:'',
            pdfUrl:'',
            allTypeUrl:{
              imgUrl:'',
              pdfUrl:'',
            }
        }
    },
    props:{
       imgUrls:{
           type:String,
           default: ''
       },
       imgType:{
           type:String,
           default: ''           
       }
       
    },
    watch:{
      allTypeUrl:{
        handler(newVal,oldVal){
          // console.log('allTypenewVal',newVal,'oldVal',oldVal);
          this.$emit('allType',newVal)  
        },
        immediate:true,
        deep:true,
      },
        imgUrls(newVal,oldVal){
          // console.log('imgUrls',newVal);
          let arr = newVal.split(".");
          let fileType = arr[arr.length - 1];
          if (fileType == "pdf" || fileType == "PDF"|| fileType == "Pdf") {
            this.allTypeUrl.imgUrl=newVal   
            this.allTypeUrl.pdfUrl =  newVal;        
            return;
          }  
            this.allTypeUrl.imgUrl = newVal     
        },
        immediate:true,
        deep:true
    },
    methods: {
        //图片缩略图
        imgPreview: function(file,fileList){
          this.loading=true
          this.typefile = file.raw.type;
          console.log('this.typefile',file.raw.type);
                if(file.raw.type !="image/png" && file.raw.type!="image/jpeg" && file.raw.type!="image/jpg" && file.raw.type!="application/pdf"){          
                    this.$message.error("请上传格式为：png、jpg、jpeg、pdf的文件！");
                    this.loading=false
                    return false
                }
                const isLt2M = file.size / 1024 / 1024 < 10;
                if(!isLt2M) {
                    this.$message.error('上传文件大小不能超过 10MB!');
                    this.loading=false
                    return false
                }
                //生成临时缩略图
              this.allTypeUrl.imgUrl = URL.createObjectURL(file.raw);
              let formData = new FormData()
              formData.append("file", file.raw)
              this.allTypeUrl.imgUrl = ''
              if (file.raw.type === "application/pdf") {
                this.allTypeUrl.imgUrl = require("../../assets/img/PDF.jpg");
              }
              uploadPicture(formData).then(Response => {
                if (Response.code == 200) {
                  this.loading=false
                  console.log('Response', Response);
                  if (file.raw.type === "application/pdf") {
                    this.allTypeUrl.imgUrl = require("../../assets/img/PDF.jpg");
                    this.allTypeUrl.pdfUrl = Response.url;
                    // this.allTypeUrl.pdfUrl = Response.originalFilename;
                    // console.log('this.pdfUrl11111', this.allTypeUrl.pdfUrl);
                    return;
                  }
                  this.allTypeUrl.imgUrl = Response.url

                }

              }).catch(res=>{
                this.loading=false
              })
                                              
        },
        enlarge: function(){
          this.loading=false
          // console.log('放大图片...',this.allTypeUrl.imgUrl);
          let arr = this.allTypeUrl.imgUrl.split(".");
          let fileType = arr[arr.length - 1];
          // console.log('arr...',fileType);
          if (fileType == "pdf" || fileType == "PDF"|| fileType == "Pdf") { 
            this.allTypeUrl.pdfUrl =  this.allTypeUrl.imgUrl;        
            window.open(this.allTypeUrl.imgUrl, "_blank");
            return;
          }          
          if (this.typefile == "application/pdf") {
            console.log('this.allTypeUrl.pdfUrl',this.allTypeUrl.pdfUrl);
            window.open(this.allTypeUrl.pdfUrl, "_blank");
            // window.open(this.allTypeUrl.imgUrl, "_blank");
            return;
          }
            this.dialogVisible = true;
            this.dialogUrl = this.allTypeUrl.imgUrl;
        },
        del: function(){
            this.loading=false
            this.allTypeUrl.imgUrl =  '';
            this.dialogUrl = '';
            this.allTypeUrl.pdfUrl = '';
        }
    },






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
.img-show{
    position: relative;
    // border: 1px solid #c0ccda;
    border-radius: 6px;
    box-sizing: border-box;
    width: 100px;
    height: 100px;
    cursor: pointer;
    overflow: hidden;
}
.uploader-avatar>>>.el-upload:hover {
    border-color: #409EFF;
}
.uploader-avatar>>>i {
    font-size: 28px;
    color: #8c939d;
}

.actions{
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
    background-color: rgba(0,0,0,.5);
    transition: opacity .3s;
}
.actions:hover{
    opacity: 1;
}
.actions:hover span{
    display: inline-block;
}
.actions span{
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
    /* .el-icon-plus:before {
    content: "";
} */
</style>
