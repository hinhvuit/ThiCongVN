<template>
  <div>
      <el-row :gutter="24">
        <el-col :span="24">
          <div>
            <div id="sign">              
                <el-timeline>
                    <el-timeline-item        
                    icon="el-icon-check"            
                    :color="(item.signStatus==1||item.signStatus==2)?'#D7D7D7':'#6091F7'"
                    v-for="(item,index) in activities" :key="index"
                    >
                      <!-- 时间线左侧 -->
                      <div class="companydel">
                        <div style="font-size: 12px;" :style="{color:item.signStatus==2?'#999999':'#222222'}">{{item.signTypeRemark}}</div>                         
                        <!-- <div>{{item.signTime}}</div> -->
                        <!-- <div style="padding-bottom:5px">2022-12-02 09:02:00</div> -->
                      </div>
                      <!-- 时间线右侧 -->
                      <div style="width: 545px;background: #F3F5F8;border-radius: 5px;">
                        <div style="padding:15px">
                          <div class="flex">
                            <!-- <div class="mr_10"><el-avatar :size="25" :src="item.headPortrait"></el-avatar></div> -->
                            <div class="user-avatar">{{ item.username.slice(0, 1) }}</div>
                            <div>
                              <div style="font-size: 16px;color: #222222;padding-bottom:10px;">
                                <div class="flex align-items-center">
                                  <div>{{item.username}}<span v-if='item.employeeno'>（{{item.employeeno}}）</span> </div>
                                  <div  style="margin:0 8px;border-radius: 4px;color:#fff;padding:3px 10px" :style="{background:backgroundcolor(item.signStatus)}" class="fs_12" v-if="item.signStatus!='2'">
                                    <dict-tag :options="sys_sign_state" :value="item.signStatus"/>
                                  </div>
                                </div>
                                <div style="font-size:12px;color: #222222;" v-if="item.agentName!=null&&item.agentName!=undefined&&item.agentName!=''">{{$t('public.Allograph')}}{{item.agentName}}<span v-if='item.agentEmployno'>（{{item.agentEmployno}}）</span></div>
                              </div>
                              <div style="font-size: 14px;color: #666666;">
                                <div style="padding-bottom:5px" v-if="item.phone!=''&&item.phone!=null&&item.phone!=undefined">{{$t('public.ContactInformation')}}{{item.phone}}</div>
                                <div style="padding-bottom:5px" v-if="item.stopTime!=''&&item.stopTime!=null&&item.stopTime!=undefined">{{$t('public.DwellTime')}}{{item.stopTime}}</div>
                                <div style="padding-bottom:5px" v-if="item.remark!=''&&item.remark!=null&&item.remark!=undefined">{{$t('public.ApprovalComments')}}{{item.remark}}</div>
                                <div>{{$t('public.ApprovalTime')}}{{item.signTime}}</div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </el-timeline-item>                               
                  </el-timeline>
            </div>            
          </div>
        </el-col>
      </el-row>
  </div>
</template>

<!-- <script>
import {getSignProcessTemplate} from '../../api/public/approvalProgress'
export default {
  name:'approvalProgress',
  dicts: ['sys_examine_type','sys_sign_state', 'sys_sign_result'],
  props:{
    businessKey:{
      type:String,
    }
  },
  watch:{
    businessKey(newVal,oldVal){
      this.getSignProcessList(newVal)
    },
    immediate:true
  },
  data() {
    return {
      activities: [], 
    }
  },
  methods: {  
    backgroundcolor(val){
      if(val=='0') return '#21B733'
      if(val=='1') return '#FFB237'
      if(val=='3') return '#FF562F'
    },
    getSignProcessList(key){
      getSignProcessTemplate(key).then(response=>{
        this.activities=response.data
        this.$emit('getSignNode',this.activities)
      })
    }


  },
  created() {
   
  },
}
</script> -->

<script setup name="approvalProgress">
import { getSignProcessTemplate } from '../../api/public/approvalProgress'

const { proxy } = getCurrentInstance();
// const { sys_examine_type } = proxy.useDict("sys_examine_type");
const { sys_sign_state } = proxy.useDict("sys_sign_state");
// const { sys_sign_result } = proxy.useDict("sys_sign_result");

const activities = ref([]);
const props = defineProps({
  businessKey: {
    type: String
  },
});

const backgroundcolor = (val) => {
  if(val=='0') return '#21B733'
  if(val=='1') return '#FFB237'
  if(val=='3') return '#FF562F'
};
const getSignProcessList = (key) => {
  getSignProcessTemplate(key).then(response=>{
    activities.value=response.data
    proxy.$emit('getSignNode',activities.value)
  })
};

watch(
  () => props.businessKey,
  (newVal, oldVal) => {
    getSignProcessList(newVal);
  },
  { immediate: true }
);

</script>

<style scoped>
#sign {
  position: relative;
  top: 10px;
  left:150px;
  width: 25%;
  /* height: 500px; */
}

.companydel {
    color: #909399;
    position: absolute;
    left: -200px;
    top: 1px;
    font-size: 12px;
    width: 200px;
    text-align: right;
    padding-right: 20px;
    overflow:hidden

}
ul.el-timeline{
  overflow: visible;
}
.el-timeline-item{
  padding-bottom: 10px;
}
.el-timeline-item__node--normal{
  width: 16px;
  height: 16px;
}
.el-timeline-item__tail{
  left: 6px;
}
::v-deep .el-avatar > img{
  width: 100%;
}
.user-avatar {
  cursor: pointer;
  width: 25px;
  height: 25px;
  border-radius: 25px;
  font-size: 16px;
  font-weight: 400;
  text-align: center;
  line-height: 25px;
  background-color: #38383838;
  margin-right: 10px;
}
</style>