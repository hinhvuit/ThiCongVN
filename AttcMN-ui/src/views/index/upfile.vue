<template>
  <div style="padding: 20px">
    <el-card>
      <div style="margin-bottom: 30px">{{ route.query.label }}</div>
      <el-form :inline="true" :model="form" class="demo-form-inline">
        <el-form-item :label="$t('public.type')" prop="indexType" label-width="150">
          {{ typeList[eidata.type - 1]?.label }}
        </el-form-item>
        <div>
          <el-form-item :label="$t('public.Operator')" label-width="150">
            {{ eidata.name }}
          </el-form-item>
          <el-form-item :label="$t('public.OperatingTime')" label-width="150">
            {{ eidata.time }}
          </el-form-item>
        </div>
      </el-form>
      <div style="margin-bottom: 30px">{{$t('public.FileInformation')}}</div>
      <el-form :inline="true" :model="form" ref="roleRef" class="demo-form-inline" :rules="rules">
        <el-form-item :label="$t('public.FlieName')" prop="title" label-width="150">
          <el-input v-model="form.title" :placeholder="$t('public.enter')" style="width: 200px" />
        </el-form-item>
        <el-form-item :label="$t('public.FileType')" prop="indexType" label-width="150" v-if="form.type == 2">
          <el-input v-model="form.indexType" :placeholder="$t('public.enter')" style="width: 200px" />
        </el-form-item>
        <div>
          <el-form-item :label="$t('public.DisplayTime')" label-width="150" prop="showTime">
            <el-date-picker v-model="form.showTime" type="date" :placeholder="$t('public.choose')" style="width: 200px" />
          </el-form-item>
          <el-form-item :label="$t('public.DisplaySequence')" prop="showOrder" label-width="150">
            <el-input v-model="form.showOrder" :placeholder="$t('public.enter')" style="width: 200px" />
          </el-form-item>
        </div>
        <div v-if="form.type == 1">
          <el-form-item :label="$t('public.CompressionDiagram')" label-width="150">
            <MyUploadImage
              :myTip="$t('public.TheSpecificationIs')"
              fileSizeMax="2 * 1024"
              :fileSizeMin="0"
              :imageUrl="form.smallFile"
              :url="form.smallFileUrl"
              @uploadChange="(file) => fileChange(file, 'smallFile')"
            >
            </MyUploadImage>
          </el-form-item>
        </div>
        <div>
          <el-form-item :label="$t('public.File')" label-width="150">
            <MyUploadFile
              :fileSizeMax="20 * 1024"
              :fileSizeMin="0"
              :fileType="['pdf']"
              :url="form.fileUrl"
              @uploadChange="(file) => fileChange(file, 'file')"
            ></MyUploadFile>
          </el-form-item>
        </div>
      </el-form>
      <el-button type="primary" @click="commit">{{ $route.query.data != undefined ? $t('public.Edit') : $t('public.submit') }}</el-button>
    </el-card>
  </div>
</template>
<script setup>
  import MyUploadFile from '@/components/MyUploadFile/index.vue';
  import MyUploadImage from '@/components/MyUploadImage/index.vue';
  import { addindexFile, updateindexFile } from '@/api/index/index';
  import useUserStore from '@/store/modules/user';
  import moment from 'moment';
  const { proxy } = getCurrentInstance();
  const router = useRouter();
  const route = useRoute();
  const data = reactive({
    form: {
      type: proxy.$route.query.type,
      indexType: '',
      title: '',
      showTime: '',
      showOrder: '',
      smallFile: '',
      file: '',
      smallFileUrl: '',
      fileUrl: '',
      id: ''
    },
    eidata: {
      type: '',
      name: '',
      time: ''
    },
    rules: {
      title: [{ required: true, message: proxy.$t('public.FlieName') + proxy.$t('public.BeEmpty'), trigger: 'blur' }],
      indexType: [{ required: true, message: proxy.$t('public.FileType') + proxy.$t('public.BeEmpty'), trigger: 'blur' }],
      showTime: [{ required: true, message: proxy.$t('public.DisplayTime') + proxy.$t('public.BeEmpty'), trigger: 'blur' }],
      showOrder: [{ required: true, message: proxy.$t('public.DisplaySequence') + proxy.$t('public.BeEmpty'), trigger: 'blur' }]
    }
  });

  const { form, eidata, rules } = toRefs(data);

  const obj = Object.assign({}, route, { title: route.query.label });
  proxy.$tab.updatePage(obj);
  const typeList = [
    { label: proxy.$t('public.SecurityDynamics'), value: 1 },
    { label: proxy.$t('public.PolicyDocument'), value: 2 },
    { label: proxy.$t('public.OperationInstruction'), value: 3 },
    // { label: '常见问题', value: 4 }
  ];
  if (proxy.$route.query.data != undefined) {
    form.value.id = JSON.parse(proxy.$route.query.data).id;
    form.value.type = JSON.parse(proxy.$route.query.data).type;
    form.value.indexType = JSON.parse(proxy.$route.query.data).indexType;
    form.value.title = JSON.parse(proxy.$route.query.data).title;
    form.value.showTime = JSON.parse(proxy.$route.query.data).showTime;
    form.value.showOrder = JSON.parse(proxy.$route.query.data).showOrder;
    form.value.smallFileUrl = JSON.parse(proxy.$route.query.data).smallFileUrl;
    form.value.fileUrl = JSON.parse(proxy.$route.query.data).fileUrl;
    eidata.value.name = JSON.parse(proxy.$route.query.data).createName;
    eidata.value.type = parseInt(route.query.type);
    eidata.value.time = JSON.parse(proxy.$route.query.data).createTime;
  } else {
    eidata.value.name = useUserStore().nameNo;
    eidata.value.type = parseInt(route.query.type);
    eidata.value.time = moment().format('YYYY-MM-DD HH:mm:ss');
  }
  function commit() {
    proxy.$refs['roleRef'].validate((valid) => {
      if (valid) {
        if (proxy.$route.query.data != undefined) {
          form.value.showTime = moment(form.value.showTime).format('YYYY-MM-DD HH:mm:ss');
          if (form.value.smallFile == '') {
            delete form.value['smallFile'];
          }
          if (form.value.file == '') {
            delete form.value['file'];
          }
          let forms = new FormData();
          for (const key in form.value) {
            forms.append(key, form.value[key]);
          }
          updateindexFile(forms).then((res) => {
            proxy.$modal.msgSuccess('修改成功');
            router.push('/system/notice?type');
          });
        } else {
          form.value.showTime = moment(form.value.showTime).format('YYYY-MM-DD HH:mm:ss');
          if (form.value.type == 1 && form.value.smallFile == '') return proxy.$modal.msgError(proxy.$t('public.CompressionDiagram') + proxy.$t('public.BeEmpty'));
          if (form.value.type == 1 && form.value.file == '') return proxy.$modal.msgError(proxy.$t('public.File') + proxy.$t('public.BeEmpty'));
          if (form.value.smallFile == '') {
            delete form.value['smallFile'];
          }
          if (form.value.file == '') {
            delete form.value['file'];
          }
          let forms = new FormData();
          for (const key in form.value) {
            forms.append(key, form.value[key]);
          }
          addindexFile(forms).then((res) => {
            proxy.$modal.msgSuccess(proxy.$t('public.Success'));
            router.push('/system/notice');
          });
        }
      }
    });
  }
  function fileChange(file, key) {
    form.value[key] = file;
  }
</script>

<style lang="scss" scoped>
  .demo-tabs > .el-tabs__content {
    padding: 32px;
    color: #6b778c;
    font-size: 32px;
    font-weight: 600;
  }

  ::v-deep(.el-form-item__label) {
    justify-content: flex-start;
  }
</style>
