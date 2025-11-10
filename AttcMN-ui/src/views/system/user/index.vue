<template>
  <div class="app-container">
    <el-row :gutter="20">
      <!--部门数据-->
      <!-- <el-col :span="4" :xs="24">
        <div class="head-container">
          <el-input
            v-model="deptName"
            placeholder="请输入部门名称"
            clearable
            prefix-icon="Search"
            style="margin-bottom: 20px"
          />
        </div>
        <div class="head-container">
          <el-tree
            :data="deptOptions"
            :props="{ label: 'label', children: 'children' }"
            :expand-on-click-node="false"
            :filter-node-method="filterNode"
            ref="deptTreeRef"
            node-key="id"
            highlight-current
            default-expand-all
            @node-click="handleNodeClick"
          />
        </div>
      </el-col> -->
      <!--用户数据-->
      <el-col :span="24" :xs="24">
        <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
          <el-form-item prop="userName">
            <el-input
              v-model="queryParams.userName"
              :placeholder="$t('public.workNumber')"
              clearable
              style="width: 240px"
              @keyup.enter="handleQuery"
            />
          </el-form-item>
          <el-form-item prop="phonenumber">
            <el-input
              v-model="queryParams.phonenumber"
              :placeholder="$t('public.PhoneNumber')"
              clearable
              style="width: 240px"
              @keyup.enter="handleQuery"
            />
          </el-form-item>
          <el-form-item prop="status">
            <el-select v-model="queryParams.status" :placeholder="$t('public.Status')" clearable style="width: 240px">
              <el-option v-for="dict in sys_normal_disable" :key="dict.value" :label="dict.label" :value="dict.value" />
            </el-select>
          </el-form-item>
          <el-form-item style="width: 280px">
            <el-date-picker
              v-model="dateRange"
              value-format="YYYY-MM-DD"
              type="daterange"
              range-separator="-"
              :start-placeholder="$t('public.StartTime')"
              :end-placeholder="$t('public.EndTime')"
            ></el-date-picker>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" icon="Search" @click="handleQuery">{{ $t('public.Query') }}</el-button>
            <el-button icon="Refresh" @click="resetQuery">{{ $t('public.Reset') }}</el-button>
          </el-form-item>
        </el-form>

        <el-row :gutter="10" class="mb8">
          <el-col :span="1.5">
            <el-button type="primary" plain icon="Plus" @click="handleAdd"
              v-hasPermi="['system:user:add']">{{$t('public.Add')}}</el-button>
          </el-col>
          <el-col :span="1.5">
            <el-button type="success" plain icon="Edit" :disabled="single" @click="handleUpdate"
              v-hasPermi="['system:user:edit']">{{$t('public.Edit')}}</el-button>
          </el-col>
          <el-col :span="1.5">
            <el-button type="danger" plain icon="Delete" :disabled="multiple" @click="handleDelete"
              v-hasPermi="['system:user:remove']">{{$t('public.delete')}}</el-button>
          </el-col>
          <!-- <el-col :span="1.5">
            <el-button type="info" plain icon="Upload" @click="handleImport" v-hasPermi="['system:user:import']"
              >导入</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button type="warning" plain icon="Download" @click="handleExport" v-hasPermi="['system:user:export']"
              >导出</el-button
            >
          </el-col> -->
          <right-toolbar v-model:showSearch="showSearch" @queryTable="getList" :columns="columns"></right-toolbar>
        </el-row>

        <el-table v-loading="loading" :data="userList" @selection-change="handleSelectionChange">
          <el-table-column type="selection" width="50" align="center" />
          <el-table-column label="ID" align="center" key="userId" prop="userId" v-if="columns[0].visible" />
          <el-table-column :label="$t('public.workNumber')" align="center" key="userName" prop="userName" v-if="columns[1].visible"
            :show-overflow-tooltip="true" />
          <el-table-column :label="$t('public.Name')" align="center" key="nickName" prop="nickName" v-if="columns[2].visible"
            :show-overflow-tooltip="true" />
          <el-table-column :label="$t('public.BelongingDepartment')" align="center" key="deptName" prop="dept.deptName" v-if="columns[3].visible"
            :show-overflow-tooltip="true" />
          <el-table-column :label="$t('public.PhoneNumber')" align="center" key="phonenumber" prop="phonenumber" v-if="columns[4].visible"
            width="120" />
          <el-table-column :label="$t('public.Status')" align="center" key="status" v-if="columns[5].visible">
            <template #default="scope">
              <el-switch v-model="scope.row.status" active-value="0" inactive-value="1"
                @change="handleStatusChange(scope.row)"></el-switch>
            </template>
          </el-table-column>
          <el-table-column :label="$t('public.CreateTime')" align="center" prop="createTime" v-if="columns[6].visible" width="160">
            <template #default="scope">
              <span>{{ parseTime(scope.row.createTime) }}</span>
            </template>
          </el-table-column>
          <el-table-column :label="$t('public.Operating')" align="center" width="150" class-name="small-padding fixed-width">
            <template #default="scope">
              <el-tooltip :content="$t('public.Edit')" placement="top" v-if="scope.row.userId !== 1">
                <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)"
                  v-hasPermi="['system:user:edit']"></el-button>
              </el-tooltip>
              <el-tooltip :content="$t('public.delete')" placement="top" v-if="scope.row.userId !== 1">
                <el-button link type="primary" icon="Delete" @click="handleDelete(scope.row)"
                  v-hasPermi="['system:user:remove']"></el-button>
              </el-tooltip>
              <el-tooltip :content="$t('public.PasswordReset')" placement="top" v-if="scope.row.userId !== 1">
                <el-button link type="primary" icon="Key" @click="handleResetPwd(scope.row)"
                  v-hasPermi="['system:user:resetPwd']"></el-button>
              </el-tooltip>
              <!-- <el-tooltip content="分配角色" placement="top" v-if="scope.row.userId !== 1">
                <el-button link type="primary" icon="CircleCheck" @click="handleAuthRole(scope.row)"
                  v-hasPermi="['system:user:edit']"></el-button>
              </el-tooltip> -->
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total > 0" :total="total" v-model:page="queryParams.pageNum"
          v-model:limit="queryParams.pageSize" @pagination="getList" />
      </el-col>
    </el-row>

    <!-- 添加或修改用户配置对话框 -->
    <el-dialog :title="title" v-model="open" width="650px" append-to-body>
      <el-form :model="form" :rules="rules" ref="userRef" label-width="130px">
        <el-row>
          <el-col :span="12">
            <el-form-item v-if="form.userId == undefined" :label="$t('public.workNumber')" prop="userName">
              <el-input v-model="form.userName" 
              :placeholder="$t('public.workNumber')" 
              maxlength="30"
               @change="getUserInfo(form.userName)"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item v-if="form.userId == undefined" :label="$t('public.PassWord')" prop="password">
              <el-input v-model="form.password" :placeholder="$t('public.PleasseEnTerPassWord')" type="password" maxlength="20" show-password />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item :label="$t('public.Name')" prop="nickName">
              <el-input v-model="form.nickName" :placeholder="$t('public.enter')" maxlength="30" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('public.Vendor')" prop="deptId">
              <el-tree-select
                v-model="form.deptId"
                :data="deptOptions"
                :props="{ value: 'id', label: 'label', children: 'children' }"
                value-key="id"
                placeholder="请选择归属部门"
                check-strictly
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('public.Roles')">
              <el-select v-model="form.roleIds" multiple :placeholder="$t('public.choose')">
                <el-option v-for="item in roleOptions" :key="item.roleId" :label="item.roleName" :value="item.roleId"
                  :disabled="item.status == 1"></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('public.Factory')">
              <el-select v-model="form.factoryIds" multiple :placeholder="$t('public.choose')">
                  <el-option v-for="item in factoryOptions" :key="item.facId" :label="item.facName" :value="item.facId"
                    :disabled="item.status == 1"></el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item :label="$t('public.PhoneNumber')" prop="phonenumber">
              <el-input v-model="form.phonenumber" :placeholder="$t('public.enter')" maxlength="11" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('public.Email')" prop="email">
              <el-input v-model="form.email" :placeholder="$t('public.enter')" maxlength="50" />
            </el-form-item>
          </el-col>
        </el-row>
        <!-- <el-row>
          
          <el-col :span="12">
            <el-form-item label="状态">
              <el-radio-group v-model="form.status">
                <el-radio v-for="dict in sys_normal_disable" :key="dict.value" :label="dict.value">{{
                  dict.label
                }}</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row> -->
        <el-row>
          <!-- <el-col :span="12">
                  <el-form-item label="用户性别">
                     <el-select v-model="form.sex" placeholder="请选择">
                        <el-option
                           v-for="dict in sys_user_sex"
                           :key="dict.value"
                           :label="dict.label"
                           :value="dict.value"
                        ></el-option>
                     </el-select>
                  </el-form-item>
               </el-col> -->

          <!-- <el-col :span="12">
                  <el-form-item label="岗位">
                     <el-select v-model="form.postIds" multiple placeholder="请选择">
                        <el-option
                           v-for="item in postOptions"
                           :key="item.postId"
                           :label="item.postName"
                           :value="item.postId"
                           :disabled="item.status == 1"
                        ></el-option>
                     </el-select>
                  </el-form-item>
               </el-col> -->
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item :label="$t('public.Remark')">
              <el-input v-model="form.remark" type="textarea" :placeholder="$t('public.enter')"></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button type="primary" @click="submitForm">{{$t('public.sure')}}</el-button>
          <el-button @click="cancel">{{$t('public.cancel')}}</el-button>
        </div>
      </template>
    </el-dialog>

    <!-- 用户导入对话框 -->
    <el-dialog :title="upload.title" v-model="upload.open" width="400px" append-to-body>
      <el-upload ref="uploadRef" :limit="1" accept=".xlsx, .xls" :headers="upload.headers"
        :action="upload.url + '?updateSupport=' + upload.updateSupport" :disabled="upload.isUploading"
        :on-progress="handleFileUploadProgress" :on-success="handleFileSuccess" :auto-upload="false" drag>
        <el-icon class="el-icon--upload"><upload-filled /></el-icon>
        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
        <template #tip>
          <div class="el-upload__tip text-center">
            <div class="el-upload__tip"><el-checkbox v-model="upload.updateSupport" />是否更新已经存在的用户数据</div>
            <span>仅允许导入xls、xlsx格式文件。</span>
            <el-link type="primary" :underline="false" style="font-size: 12px; vertical-align: baseline"
              @click="importTemplate">下载模板</el-link>
          </div>
        </template>
      </el-upload>
      <template #footer>
        <div class="dialog-footer">
          <el-button type="primary" @click="submitFileForm">确 定</el-button>
          <el-button @click="upload.open = false">取 消</el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="User">
import { getToken } from '@/utils/auth';
import {
  changeUserStatus,
  listUser,
  resetUserPwd,
  delUser,
  getUser,
  updateUser,
  addUser,
  deptTreeSelect,
  Business
} from '@/api/system/user';
import { ref, reactive, computed, onMounted, watch, toRefs } from 'vue';
import useUserStore from '@/store/modules/user'

const router = useRouter();
const { proxy } = getCurrentInstance();
const { sys_normal_disable, sys_user_sex } = proxy.useDict('sys_normal_disable', 'sys_user_sex');

const userList = ref([]);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const ids = ref([]);
const single = ref(true);
const multiple = ref(true);
const total = ref(0);
const title = ref('');
const dateRange = ref([]);
const deptName = ref('');
const deptOptions = ref(undefined);
const initPassword = ref(undefined);
const postOptions = ref([]);
const roleOptions = ref([]);
const factoryOptions = ref([]);
const userStore = useUserStore()
const factories = computed(() => userStore.factories || [])

// keep factoryOptions in sync with user store factories
watch(factories, (val) => {
  factoryOptions.value = val || []
}, { immediate: true })
/*** 用户导入参数 */
const upload = reactive({
  // 是否显示弹出层（用户导入）
  open: false,
  // 弹出层标题（用户导入）
  title: '',
  // 是否禁用上传
  isUploading: false,
  // 是否更新已经存在的用户数据
  updateSupport: 0,
  // 设置上传的请求头部
  headers: { Authorization: 'Bearer ' + getToken() },
  // 上传的地址
  url: import.meta.env.VITE_APP_BASE_API + '/system/user/importData'
});
// 列显隐信息
const columns = ref([
  { key: 0, label: `用户编号`, visible: true },
  { key: 1, label: `工号`, visible: true },
  { key: 2, label: `姓名`, visible: true },
  { key: 3, label: `部门`, visible: true },
  { key: 4, label: `手机号码`, visible: true },
  { key: 5, label: `状态`, visible: true },
  { key: 6, label: `创建时间`, visible: true }
]);

const data = reactive({
  form: {},
  queryParams: {
    pageNum: 1,
    pageSize: 10,
    userName: undefined,
    phonenumber: undefined,
    status: undefined,
    deptId: undefined
  },
  rules: {
    userName: [
      { required: true, message: proxy.$t('public.workNumberRemind'), trigger: 'blur' },
      // { min: 2, max: 20, message: '工号长度必须介于 2 和 20 之间', trigger: 'blur' }
    ],
    nickName: [{ required: true, message: proxy.$t('public.nameRemind'), trigger: 'blur' }],
    password: [
      { required: true, message: proxy.$t('public.passwordEmpty'), trigger: 'blur' },
      // { min: 5, max: 20, message: '密码长度必须介于 5 和 20 之间', trigger: 'blur' },
      { pattern: /^[^<>"'|\\]+$/, message: proxy.$t('public.illegalCharacters')+'：< > " \' \\\ |', trigger: 'blur' }
    ],
    email: [{ type: 'email', message: proxy.$t('public.emailFormat'), trigger: ['blur', 'change'] }],
    // phonenumber: [{ pattern: /^1[3|4|5|6|7|8|9][0-9]\d{8}$/, message: '请输入正确的手机号码', trigger: 'blur' }]
  }
});

const { queryParams, form, rules } = toRefs(data);

/** 通过条件过滤节点  */
const filterNode = (value, data) => {
  if (!value) return true;
  return data.label.indexOf(value) !== -1;
};
/** 根据名称筛选部门树 */
watch(deptName, (val) => {
  proxy.$refs['deptTreeRef'].filter(val);
});
/** 查询部门下拉树结构 */
function getDeptTree() {
  deptTreeSelect().then((response) => {
    deptOptions.value = response.data;
  });
}
/** 查询用户列表 */
function getList() {
  loading.value = true;
  listUser(proxy.addDateRange(queryParams.value, dateRange.value)).then((res) => {
    loading.value = false;
    userList.value = res.rows;
    total.value = res.total;
  });
}
/** 节点单击事件 */
function handleNodeClick(data) {
  queryParams.value.deptId = data.id;
  handleQuery();
}
/** 搜索按钮操作 */
function handleQuery() {
  queryParams.value.pageNum = 1;
  getList();
}
/** 重置按钮操作 */
function resetQuery() {
  dateRange.value = [];
  proxy.resetForm('queryRef');
  queryParams.value.deptId = undefined;
  proxy.$refs.deptTreeRef.setCurrentKey(null);
  handleQuery();
}
/** 删除按钮操作 */
function handleDelete(row) {
  const userIds = row.userId || ids.value;
  proxy.$modal
    .confirm(proxy.$t('public.DeleteOrNot') + userIds + '？')
    .then(function () {
      return delUser(userIds);
    })
    .then(() => {
      getList();
      proxy.$modal.msgSuccess(proxy.$t('public.Success'));
    })
    .catch(() => { });
}
/** 导出按钮操作 */
function handleExport() {
  proxy.download(
    'system/user/export',
    {
      ...queryParams.value
    },
    `user_${new Date().getTime()}.xlsx`
  );
}
/** 用户状态修改  */
function handleStatusChange(row) {
  let text = row.status === '0' ? proxy.$t('public.Enable') : proxy.$t('public.Disuse');
  proxy.$modal
    .confirm(text + '""' + row.userName + '"?')
    .then(function () {
      return changeUserStatus(row.userId, row.status);
    })
    .then(() => {
      proxy.$modal.msgSuccess(text +  proxy.$t('public.Success'));
    })
    .catch(function () {
      row.status = row.status === '0' ? '1' : '0';
    });
}
/** 更多操作 */
function handleCommand(command, row) {
  switch (command) {
    case 'handleResetPwd':
      handleResetPwd(row);
      break;
    case 'handleAuthRole':
      handleAuthRole(row);
      break;
    default:
      break;
  }
}
/** 跳转角色分配 */
function handleAuthRole(row) {
  const userId = row.userId;
  router.push('/system/user-auth/role/' + userId);
}
/** 重置密码按钮操作 */
function handleResetPwd(row) {
  proxy
    .$prompt(proxy.$t('public.PleaseInput') + row.userName + proxy.$t('public.NewPassword'), proxy.$t('public.Tips'), {
      confirmButtonText: proxy.$t('public.sure'),
      cancelButtonText: proxy.$t('public.cancel'),
      closeOnClickModal: false,
      inputPattern: /^.{5,20}$/,
      inputErrorMessage: proxy.$t('public.NewPasswordTips'),
      inputValidator: (value) => {
        if (/<|>|"|'|\||\\/.test(value)) {
          return proxy.$t('public.illegalCharacters')+`：< > " \' \\\ |`;
        }
      }
    })
    .then(({ value }) => {
      resetUserPwd(row.userId, value).then((response) => {
        proxy.$modal.msgSuccess(proxy.$t('public.NewPasswordTips2') + value);
      });
    })
    .catch(() => { });
}
/** 选择条数  */
function handleSelectionChange(selection) {
  ids.value = selection.map((item) => item.userId);
  single.value = selection.length != 1;
  multiple.value = !selection.length;
}
/** 导入按钮操作 */
function handleImport() {
  upload.title = '用户导入';
  upload.open = true;
}
/** 下载模板操作 */
function importTemplate() {
  proxy.download('system/user/importTemplate', {}, `user_template_${new Date().getTime()}.xlsx`);
}
/**文件上传中处理 */
const handleFileUploadProgress = (event, file, fileList) => {
  upload.isUploading = true;
};
/** 文件上传成功处理 */
const handleFileSuccess = (response, file, fileList) => {
  upload.open = false;
  upload.isUploading = false;
  proxy.$refs['uploadRef'].handleRemove(file);
  proxy.$alert(
    "<div style='overflow: auto;overflow-x: hidden;max-height: 70vh;padding: 10px 20px 0;'>" +
    response.msg +
    '</div>',
    '导入结果',
    { dangerouslyUseHTMLString: true }
  );
  getList();
};
/** 提交上传文件 */
function submitFileForm() {
  proxy.$refs['uploadRef'].submit();
}
/** 重置操作表单 */
function reset() {
  form.value = {
    userId: undefined,
    deptId: undefined,
    userName: undefined,
    nickName: undefined,
    password: undefined,
    phonenumber: undefined,
    email: undefined,
    sex: undefined,
    status: '0',
    remark: undefined,
    postIds: [],
    roleIds: [],
    factoryIds: []
  };
  proxy.resetForm('userRef');
}
/** 取消按钮 */
function cancel() {
  open.value = false;
  reset();
}
/** 新增按钮操作 */
function handleAdd() {
  reset();
  getUser().then((response) => {
    postOptions.value = response.posts;
    roleOptions.value = response.roles;
    open.value = true;
    title.value = proxy.$t('public.Add');
    form.value.password = initPassword.value;
  });
}
/** 修改按钮操作 */
function handleUpdate(row) {
  reset();
  const userId = row.userId || ids.value;
  getUser(userId).then((response) => {
    form.value = response.data;
    postOptions.value = response.posts;
    roleOptions.value = response.roles;
    form.value.postIds = response.postIds;
    form.value.roleIds = response.roleIds;
    // populate factoryIds if returned from API
    form.value.factoryIds = response.factoryIds || response.data.factoryIds || response.data.factoryIds || form.value.factoryIds || [];
      // populate factoryIds (coerce to numbers) if present
      const rawFactoryIds = response.factoryIds || (response.data && response.data.factoryIds) || form.value.factoryIds || [];
      if (Array.isArray(rawFactoryIds)) {
        form.value.factoryIds = rawFactoryIds.map(f => {
          const n = Number(f);
          return Number.isNaN(n) ? f : n;
        });
      }
    open.value = true;
    title.value = proxy.$t('public.Edit');
    form.password = '';
  });
}
/** 提交按钮 */
function submitForm() {
  proxy.$refs['userRef'].validate((valid) => {
    if (valid) {
      // sanitize payload before sending to backend
      const payload = { ...form.value };

      // Normalize factoryIds: ensure integers and remove null/empty
      if (Array.isArray(payload.factoryIds)) {
        payload.factoryIds = payload.factoryIds
          .map((f) => {
            // If the fileUpload used combined string format, it may produce strings; coerce to number
            const n = Number(f);
            return Number.isNaN(n) ? null : n;
          })
          .filter((f) => f !== null && f !== undefined);
        if (payload.factoryIds.length === 0) {
          delete payload.factoryIds;
        }
      }

      // Ensure deptId is number or removed
      if (payload.deptId !== undefined && payload.deptId !== null && payload.deptId !== '') {
        const d = Number(payload.deptId);
        if (!Number.isNaN(d)) payload.deptId = d;
      } else {
        delete payload.deptId;
      }

      if (payload.userId != undefined) {
        updateUser(payload).then((response) => {
          proxy.$modal.msgSuccess(response.msg);
          open.value = false;
          getList();
        });
      } else {
        addUser(payload).then((response) => {
          proxy.$modal.msgSuccess(response.msg);
          open.value = false;
          getList();
        });
      }
    }
  });
}
function getUserInfo(no) {
    if (!no) return;
    getEmpInfo(no).then((res) => {
      if (res.data) {
        form.value.nickName = res.data.empName;
        form.value.phonenumber = res.data.phone;
        form.value.email = res.data.email;
      } else {
        proxy.$modal.msgWarning(proxy.$t('public.NoInformation'));
      }
    });
  }

// watch(
//   () => form.value.userName,
//   (newValue, oldValue) => {
//     Business(newValue).then((res) => {
//       form.value.nickName=res.data.empName;
//       form.value.phonenumber=res.data.phone;
//     })
//   },
//   { deep: true }
// );
getDeptTree();
getList();
</script>
