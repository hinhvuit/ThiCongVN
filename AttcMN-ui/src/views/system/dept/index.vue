<template>
   <div class="app-container">
      <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch">
         <el-form-item :label="$t('dept.deptName')" prop="deptName">
            <el-input
               v-model="queryParams.deptName"
               :placeholder="$t('dept.pleaseInputDeptName')"
               clearable
               style="width: 200px"
               @keyup.enter="handleQuery"
            />
         </el-form-item>
         <el-form-item :label="$t('common.status')" prop="status">
            <el-select v-model="queryParams.status" :placeholder="$t('dept.deptStatus')" clearable style="width: 200px">
               <el-option
                  v-for="dict in sys_normal_disable"
                  :key="dict.value"
                  :label="dict.label"
                  :value="dict.value"
               />
            </el-select>
         </el-form-item>
         <el-form-item>
            <el-button type="primary" icon="Search" @click="handleQuery">{{ $t('common.search') }}</el-button>
            <el-button icon="Refresh" @click="resetQuery">{{ $t('common.reset') }}</el-button>
         </el-form-item>
      </el-form>

      <el-row :gutter="10" class="mb8">
         <el-col :span="1.5">
            <el-button
               type="primary"
               plain
               icon="Plus"
               @click="handleAdd"
               v-hasPermi="['system:dept:add']"
            >{{ $t('common.add') }}</el-button>
         </el-col>
         <el-col :span="1.5">
            <el-button
               type="info"
               plain
               icon="Sort"
               @click="toggleExpandAll"
            >{{ $t('dept.expandCollapse') }}</el-button>
         </el-col>
         <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
      </el-row>

      <el-table
         v-if="refreshTable"
         v-loading="loading"
         :data="deptList"
         row-key="deptId"
         :default-expand-all="isExpandAll"
         :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
      >
         <el-table-column prop="deptName" :label="$t('dept.deptName')" width="260"></el-table-column>
         <el-table-column prop="orderNum" :label="$t('dept.orderNum')" width="200"></el-table-column>
         <el-table-column prop="status" :label="$t('common.status')" width="100">
            <template #default="scope">
               <dict-tag :options="sys_normal_disable" :value="scope.row.status" />
            </template>
         </el-table-column>
         <el-table-column :label="$t('common.createTime')" align="center" prop="createTime" width="200">
            <template #default="scope">
               <span>{{ parseTime(scope.row.createTime) }}</span>
            </template>
         </el-table-column>
         <el-table-column :label="$t('common.actions')" align="center" class-name="small-padding fixed-width">
            <template #default="scope">
               <el-button link type="primary" icon="Edit" @click="handleUpdate(scope.row)" v-hasPermi="['system:dept:edit']">{{ $t('common.edit') }}</el-button>
               <el-button link type="primary" icon="Plus" @click="handleAdd(scope.row)" v-hasPermi="['system:dept:add']">{{ $t('common.add') }}</el-button>
               <el-button v-if="scope.row.parentId != 0" link type="primary" icon="Delete" @click="handleDelete(scope.row)" v-hasPermi="['system:dept:remove']">{{ $t('common.delete') }}</el-button>
            </template>
         </el-table-column>
      </el-table>

      <!-- 添加或修改部门对话框 -->
      <el-dialog :title="title" v-model="open" width="600px" append-to-body>
         <el-form ref="deptRef" :model="form" :rules="rules" label-width="80px">
            <el-row>
               <el-col :span="24" v-if="form.parentId !== 0">
                  <el-form-item :label="$t('dept.parentDept')" prop="parentId">
                     <el-tree-select
                        v-model="form.parentId"
                        :data="deptOptions"
                        :props="{ value: 'deptId', label: 'deptName', children: 'children' }"
                        value-key="deptId"
                        :placeholder="$t('dept.selectParentDept')"
                        check-strictly
                     />
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item :label="$t('dept.deptName')" prop="deptName">
                     <el-input v-model="form.deptName" :placeholder="$t('dept.pleaseInputDeptName')" />
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item :label="$t('dept.displayOrder')" prop="orderNum">
                     <el-input-number v-model="form.orderNum" controls-position="right" :min="0" />
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item :label="$t('dept.leader')" prop="leader">
                     <el-input v-model="form.leader" :placeholder="$t('dept.pleaseInputLeader')" maxlength="20" />
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item :label="$t('dept.phone')" prop="phone">
                     <el-input v-model="form.phone" :placeholder="$t('dept.pleaseInputPhone')" maxlength="11" />
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item :label="$t('dept.email')" prop="email">
                     <el-input v-model="form.email" :placeholder="$t('dept.pleaseInputEmail')" maxlength="50" />
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item :label="$t('dept.deptStatus')">
                     <el-radio-group v-model="form.status">
                        <el-radio
                           v-for="dict in sys_normal_disable"
                           :key="dict.value"
                           :label="dict.value"
                        >{{ dict.label }}</el-radio>
                     </el-radio-group>
                  </el-form-item>
               </el-col>
            </el-row>
         </el-form>
         <template #footer>
            <div class="dialog-footer">
               <el-button type="primary" @click="submitForm">{{ $t('common.confirm') }}</el-button>
               <el-button @click="cancel">{{ $t('common.cancel') }}</el-button>
            </div>
         </template>
      </el-dialog>
   </div>
</template>

<script setup name="Dept">
import { listDept, getDept, delDept, addDept, updateDept, listDeptExcludeChild } from "@/api/system/dept";
import { useI18n } from 'vue-i18n';

const { proxy } = getCurrentInstance();
const { t } = useI18n();
const { sys_normal_disable } = proxy.useDict("sys_normal_disable");

const deptList = ref([]);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const title = ref("");
const deptOptions = ref([]);
const isExpandAll = ref(true);
const refreshTable = ref(true);

const data = reactive({
  form: {},
  queryParams: {
    deptName: undefined,
    status: undefined
  },
  rules: {
    parentId: [{ required: true, message: t('dept.parentDeptRequired'), trigger: "blur" }],
    deptName: [{ required: true, message: t('dept.deptNameRequired'), trigger: "blur" }],
    orderNum: [{ required: true, message: t('dept.orderNumRequired'), trigger: "blur" }],
    email: [{ type: "email", message: t('dept.emailInvalid'), trigger: ["blur", "change"] }],
    phone: [{ pattern: /^1[3|4|5|6|7|8|9][0-9]\d{8}$/, message: t('dept.phoneInvalid'), trigger: "blur" }]
  },
});

const { queryParams, form, rules } = toRefs(data);

/** 查询部门列表 */
function getList() {
  loading.value = true;
  listDept(queryParams.value).then(response => {
    deptList.value = proxy.handleTree(response.data, "deptId");
    loading.value = false;
  });
}
/** 取消按钮 */
function cancel() {
  open.value = false;
  reset();
}
/** 表单重置 */
function reset() {
  form.value = {
    deptId: undefined,
    parentId: undefined,
    deptName: undefined,
    orderNum: 0,
    leader: undefined,
    phone: undefined,
    email: undefined,
    status: "0"
  };
  proxy.resetForm("deptRef");
}
/** 搜索按钮操作 */
function handleQuery() {
  getList();
}
/** 重置按钮操作 */
function resetQuery() {
  proxy.resetForm("queryRef");
  handleQuery();
}
/** 新增按钮操作 */
function handleAdd(row) {
  reset();
  listDept().then(response => {
    deptOptions.value = proxy.handleTree(response.data, "deptId");
  });
  if (row != undefined) {
    form.value.parentId = row.deptId;
  }
  open.value = true;
  title.value = t('dept.addDept');
}
/** 展开/折叠操作 */
function toggleExpandAll() {
  refreshTable.value = false;
  isExpandAll.value = !isExpandAll.value;
  nextTick(() => {
    refreshTable.value = true;
  });
}
/** 修改按钮操作 */
function handleUpdate(row) {
  reset();
  listDeptExcludeChild(row.deptId).then(response => {
    deptOptions.value = proxy.handleTree(response.data, "deptId");
  });
  getDept(row.deptId).then(response => {
    form.value = response.data;
    open.value = true;
    title.value = t('dept.editDept');
  });
}
/** 提交按钮 */
function submitForm() {
  proxy.$refs["deptRef"].validate(valid => {
    if (valid) {
      if (form.value.deptId != undefined) {
        updateDept(form.value).then(response => {
          proxy.$modal.msgSuccess(t('common.updateSuccess'));
          open.value = false;
          getList();
        });
      } else {
        addDept(form.value).then(response => {
          proxy.$modal.msgSuccess(t('common.addSuccess'));
          open.value = false;
          getList();
        });
      }
    }
  });
}
/** 删除按钮操作 */
function handleDelete(row) {
  proxy.$modal.confirm(t('dept.confirmDelete', { name: row.deptName })).then(function() {
    return delDept(row.deptId);
  }).then(() => {
    getList();
    proxy.$modal.msgSuccess(t('common.deleteSuccess'));
  }).catch(() => {});
}

getList();
</script>
