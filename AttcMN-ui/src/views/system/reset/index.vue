<!-- 
  @FileDescription:重置用户密码
  @Date:2024-08-16 14:18:50
  @Author:LZW
-->
<template>
  <div class="app-container">
    <el-form :model="queryParams" ref="queryRef" :inline="true">
      <div style="display: flex">
        <div style="flex: 1">
          <el-form-item prop="userName">
            <el-input
              v-model="queryParams.userName"
              :placeholder="$t('public.workNumber')" 
              clearable
              style="width: 200px"
              @keyup.enter="handleQuery"
            />
          </el-form-item>
          <el-form-item prop="type">
            <el-select
              v-model="resetForm.type"
              :placeholder="$t('public.type')"
              clearable
              style="width: 200px"
              @change="selectChange"
              
            >
              <el-option v-for="(item,index) in  arr" :label=item.label :value=item.value :key="index"  />
              <!-- <el-option label="前台用户" :value="1" />
              <el-option label="园区后台用户" :value="2" /> -->
            </el-select>
          </el-form-item>
        </div>
        <div style="width: 180px; display: flex; justify-content: flex-end; align-items: flex-end">
          <el-form-item style="margin-right: 0">
            <el-button type="primary" @click="handleQuery">{{$t('public.Query')}}</el-button>
            <el-button @click="resetQuery">{{$t('public.Reset')}}</el-button>
          </el-form-item>
        </div>
      </div>
    </el-form>

    <el-table v-loading="loading" :data="listData"> 
      <el-table-column :label="$t('public.SerialNumber')" align="center" type="index" width="150" />
      <el-table-column :label="$t('public.user')" align="center" key="createBy">
        <template #default="scope"> {{ scope.row.nickName }}({{ scope.row.userName }}) </template>
      </el-table-column>
      <el-table-column :label="$t('public.CreateTime')" align="center" prop="createTime">
        <template #default="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span> 
        </template>
      </el-table-column>
      <el-table-column :label="$t('public.loginDate')" align="center" prop="loginDate">
        <template #default="scope">
          <span>{{ parseTime(scope.row.loginDate) }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('public.Operating')" align="center" width="150" class-name="small-padding fixed-width">
        <template #default="scope">
          <el-button link type="primary" icon="Key" @click="handleReset(scope.row)">{{ $t('public.PasswordReset') }}</el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination
      v-show="total > 0"
      :total="total"
      v-model:page="queryParams.pageNum"
      v-model:limit="queryParams.pageSize"
      @pagination="getList"
    />

    <el-dialog v-model="dialogFormVisible" :title="$t('public.PasswordReset')+ `:${title}`" width="500">
      <el-form :model="resetForm">
        <el-form-item :label="$t('public.Reason')" :rules="{ required: true, message: '请输入原因', trigger: 'blur' }" prop="meno">
          <el-input v-model="resetForm.meno" />
        </el-form-item>
      </el-form>
      <div class="tip">{{ $t('public.Tips') }}：<span>{{$t('public.AfterResetPassword')}}</span></div>
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogFormVisible = false"> {{$t('public.cancel')}} </el-button>
          <el-button type="primary" @click="handleSubmit"> {{$t('public.sure')}} </el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="SystemReset">
  import { getFrontUser, getBackUser, resetPassword } from '@/api/system/reset';
  const { proxy } = getCurrentInstance();

  const dialogFormVisible = ref(false);
  const title = ref(null);
  const loading = ref(false);
  const listData = ref([]);
  const total = ref(0);
  const queryParams = ref({
    pageNum: 1,
    pageSize: 10,
    userName: null
  });
  const resetForm = ref({
    userName: '',
    password: null,
    type: 1,
    meno: ''
  });
  const arr = ref([
    { label: proxy.$t('public.ForegroundUser'), value: 1 },
    { label: proxy.$t('public.BackstageUser'), value: 2 },
  ]);

  function getFront() {
    getFrontUser(queryParams.value).then((res) => {
      total.value = res.data.total;
      listData.value = res.data.records;
      loading.value = false;
    });
  }
  function getBack() {
    getBackUser(queryParams.value).then((res) => {
      total.value = res.data.total;
      listData.value = res.data.records;
      loading.value = false;
    });
  }

  function getList() {
    loading.value = true;
    if (resetForm.value.type === 1) {
      getFront();
    } else {
      getBack();
    }
  }
  function handleQuery() {
    if (!queryParams.value.userName) {
      proxy.$modal.msgError(proxy.$t('public.EnterWorkNumer'));
      return;
    }
    getList();
  }
  function resetQuery() {
    listData.value = [];
    total.value = 0;
    proxy.resetForm('queryRef');
  }

  function selectChange() {
    listData.value = [];
    total.value = 0;
  }
  const handleReset = (row) => {
    const { nickName, userName } = row;
    title.value = `${nickName}(${userName})`;
    resetForm.value.userName = userName;
    resetForm.value.meno = '';
    dialogFormVisible.value = true;
  };

  const handleSubmit = () => {
    resetPassword(resetForm.value).then((res) => {
      proxy.$modal.msgSuccess(proxy.$t('public.ResetPasswordSuccess'));
      dialogFormVisible.value = false;
    });
  };
</script>
<style lang="scss" scoped>
  .tip {
    margin-top: 30px;
    padding-left: 50px;
    span {
      font-size: 12px;
      color: #fc4141;
    }
  }
</style>
