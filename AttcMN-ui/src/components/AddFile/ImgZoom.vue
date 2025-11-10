<template>
  <div>
    <el-dialog
      title=""
      :visible.sync="ImgZoomShow"
      width="30%"
      :before-close="imgClose"
    >
      <div class="imgBigdiv">
       
        <img :src="imgBigimg" class="txt_cr" />
        </div>
    </el-dialog>
  </div>
</template>
<script>
export default {
  name: "ImgZoom",
  props: {
    imgBig: {
      type: Boolean,
      required: true,
    },
    imgBigimg: {
      type: String,
      required: true,
    },
  }, 
  data(){
      return {
          ImgZoomShow:this.imgBig
      }
  },
  watch:{
      imgBig(newVal, oldVal) {
          this.ImgZoomShow = newVal;
      }
  },
  methods: {
    // 关闭放大图片
    imgClose() {
      this.$emit("imgClose");
    },
  },
};
</script>