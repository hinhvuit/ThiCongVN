<template>
  <div>
    <img
        v-if="checkPdf(picUrl)"
        class="cursor"                
        style="width: 100px; height: 100px;border-radius: 6px;border: 1px dashed #d9d9d9;"
        src="../../assets/img/PDF.jpg"
        @click="openFile(picUrl)"
        >
    <div v-else>
      <el-image
          v-if="picUrl !== undefined"
          lazy                         
          style="width: 100px; height: 100px;border-radius: 6px;border: 1px dashed #d9d9d9;"
          :src="picUrl"
          :preview-src-list="[picUrl]"                       
          fit="cover"
          >
      </el-image>                        
    </div>      
  </div>
</template>

<script>
export default {
    props:{
        picUrl:String,
        default:''
    },
    methods:{
        checkPdf(val){
          if(val){
            let arr = val.split(".");
            let fileType = arr[arr.length - 1];
            if (fileType == "pdf" || fileType == "PDF"|| fileType == "Pdf") {
                return true
            }
          }
        }, 
        openFile(val){
          window.open(val,'_blank')
        },       
    }
}
</script>

<style>

</style>