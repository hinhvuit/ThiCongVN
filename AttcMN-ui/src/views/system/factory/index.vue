<template>
  <div class="app-container">
    <!-- Search Form -->
    <el-form :model="queryParams" ref="queryFormRef" :inline="true" v-show="showSearch">
      <el-form-item :label="$t('system.factoryName')" prop="facName">
        <el-input
          v-model="queryParams.facName"
          :placeholder="$t('system.pleaseEnterFactoryName')"
          clearable
          style="width: 240px"
          @keyup.enter="handleQuery"
        />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" :icon="Search" @click="handleQuery">{{ $t('common.search') }}</el-button>
        <el-button :icon="Refresh" @click="resetQuery">{{ $t('common.reset') }}</el-button>
      </el-form-item>
    </el-form>

    <!-- Toolbar -->
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button type="primary" plain :icon="Plus" @click="handleAdd">{{ $t('common.add') }}</el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="success" plain :icon="Edit" :disabled="single" @click="handleUpdate">{{ $t('common.edit') }}</el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="danger" plain :icon="Delete" :disabled="multiple" @click="handleDelete">{{ $t('common.delete') }}</el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <!-- Data Table -->
    <el-table v-loading="loading" :data="factoryList" @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column label="No" type="index" width="50" align="center" />
      <el-table-column :label="$t('system.factoryId')" align="center" prop="facId" width="100" />
      <el-table-column :label="$t('system.factoryName')" align="center" prop="facName" width="200" show-overflow-tooltip />
  <el-table-column :label="$t('common.shortName')" align="center" prop="facShort" width="150" />
  <el-table-column :label="$t('system.address')" align="center" prop="address" min-width="200" show-overflow-tooltip />
      <el-table-column :label="$t('common.status')" align="center" width="100">
        <template #default="scope">
          <el-tag v-if="scope.row.isDeleted === '0'" type="success">{{ $t('common.active') }}</el-tag>
          <el-tag v-else type="danger">{{ $t('common.inactive') }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column :label="$t('common.createTime')" align="center" prop="createTime" width="160" />
      <el-table-column :label="$t('common.actions')" align="center" width="180" fixed="right">
        <template #default="scope">
          <el-button link type="primary" :icon="Edit" @click="handleUpdate(scope.row)">{{ $t('common.edit') }}</el-button>
          <el-button link type="danger" :icon="Delete" @click="handleDelete(scope.row)">{{ $t('common.delete') }}</el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- Pagination -->
    <pagination
      v-show="total > 0"
      :total="total"
      v-model:page="queryParams.pageNum"
      v-model:limit="queryParams.pageSize"
      @pagination="getList"
    />

    <!-- Add/Edit Dialog -->
    <el-dialog :title="dialogTitle" v-model="dialogVisible" width="600px" append-to-body>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="120px">
        <el-form-item :label="$t('system.factoryName')" prop="facName">
          <el-input v-model="form.facName" :placeholder="$t('system.pleaseEnterFactoryName')" />
        </el-form-item>
        <el-form-item :label="$t('common.shortName')" prop="facShort">
          <el-input v-model="form.facShort" :placeholder="$t('system.pleaseEnterShortName')" />
        </el-form-item>
        <el-form-item :label="$t('system.address')" prop="address">
          <el-input v-model="form.address" :placeholder="$t('system.pleaseEnterAddress')" />
        </el-form-item>
        <el-form-item :label="$t('common.status')" prop="status">
          <el-radio-group v-model="form.isDeleted">
            <el-radio label="0">{{ $t('common.active') }}</el-radio>
            <el-radio label="1">{{ $t('common.inactive') }}</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item :label="$t('common.remark')" prop="remark">
          <el-input v-model="form.remark" type="textarea" :rows="3" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">{{ $t('btn.cancel') }}</el-button>
        <el-button type="primary" @click="submitForm">{{ $t('btn.confirm') }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="Factory">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Search, Refresh, Plus, Edit, Delete } from '@element-plus/icons-vue'
import { useI18n } from 'vue-i18n'
import {
  listFactory,
  getFactory,
  addFactory,
  updateFactory,
  delFactory
} from '@/api/construction/factory'

const { t } = useI18n()

// State
const loading = ref(false)
const showSearch = ref(true)
const factoryList = ref([])
const total = ref(0)
const dialogVisible = ref(false)
const dialogTitle = ref('')
const single = ref(true)
const multiple = ref(true)
const ids = ref([])
const originalFactory = ref(null)

// Refs
const queryFormRef = ref()
const formRef = ref()

// Query params
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  facName: null
})

// Form data
const form = reactive({
  facId: null,
  facName: '',
  facShort: '',
  address: '',
  status: '0',
  remark: ''
})

// Form validation rules
const rules = {
  facName: [{ required: true, message: t('system.pleaseEnterFactoryName'), trigger: 'blur' }],
  facCode: [{ required: true, message: t('system.pleaseEnterFactoryCode'), trigger: 'blur' }]
}

// Methods
// normalize a factory item from backend to the fields used by this view
const normalizeFactory = (item) => {
  if (!item) return {}
  return {
    // support facID (uppercase D) or facId
    facId: item.facId ?? item.facID ?? item.facID ?? null,
    facName: item.facName ?? item.name ?? '',
    facCode: item.facCode ?? item.shortName ?? item.short_name ?? '',
    address: item.address ?? item.facAddress ?? '',
  // keep contact/phone if present but UI no longer displays them
  contact: item.contact ?? '',
  phone: item.phone ?? '',
  // short name / alias
  facShort: item.facShort ?? item.shortName ?? item.short_name ?? item.facCode ?? '' ,
    status: item.status ?? '',
    // normalize isDeleted so that template checks like `scope.row.isDeleted === '0'` work
    // backend may return boolean, number or string. We normalize false/0/'0' -> '0' (active), others -> '1'
    isDeleted: (item.isDeleted === false || item.isDeleted === 0 || item.isDeleted === '0') ? '0' : '1',
    createTime: item.createTime ?? item.create_time ?? null,
    remark: item.remark ?? ''
  }
}

const getList = () => {
  loading.value = true
  listFactory(queryParams).then(response => {
    // response may be { rows, total } or { data: [...] } or an array
    let list = []
    let count = 0
    if (!response) {
      list = []
      count = 0
    } else if (Array.isArray(response)) {
      list = response
      count = response.length
    } else if (Array.isArray(response.data)) {
      list = response.data
      count = response.data.length
    } else if (Array.isArray(response.rows)) {
      list = response.rows
      count = response.total || response.rows.length
    } else if (Array.isArray(response.data?.rows)) {
      list = response.data.rows
      count = response.data.total || response.data.rows.length
    } else {
      // fallback: try response.data if it's an object with items
      list = []
      count = 0
    }

    // normalize items to expected keys
    factoryList.value = list.map(i => normalizeFactory(i))
    total.value = count
    loading.value = false
  }).catch(() => {
    loading.value = false
  })
}

const handleQuery = () => {
  queryParams.pageNum = 1
  getList()
}

const resetQuery = () => {
  queryFormRef.value?.resetFields()
  handleQuery()
}

const handleSelectionChange = (selection) => {
  ids.value = selection.map(item => item.facId)
  single.value = selection.length !== 1
  multiple.value = !selection.length
}

const handleAdd = () => {
  reset()
  dialogVisible.value = true
  dialogTitle.value = t('common.add') + ' ' + t('system.factory')
}

const handleUpdate = (row) => {
  reset()
  const facId = row?.facId || ids.value[0]
  getFactory(facId).then(response => {
    // response.data may use different keys; normalize before assigning
    const data = response.data ?? response
    // keep raw response so we can merge unchanged fields on update
    originalFactory.value = data
    Object.assign(form, normalizeFactory(data))
    // keep the original facId on the form (server may expect facId when updating)
    form.facId = data.facId ?? data.facID ?? data.facID ?? form.facId
    dialogVisible.value = true
    dialogTitle.value = t('common.edit') + ' ' + t('system.factory')
  })
}

const handleDelete = (row) => {
  const facIds = row.facId ? [row.facId] : ids.value
  ElMessageBox.confirm(
    t('common.confirmDelete'),
    t('common.tips'),
    {
      confirmButtonText: t('btn.confirm'),
      cancelButtonText: t('btn.cancel'),
      type: 'warning'
    }
  ).then(() => {
    return delFactory(facIds[0])
  }).then(() => {
    getList()
    ElMessage.success(t('common.deleteSuccess'))
  }).catch(() => {})
}

const submitForm = () => {
  formRef.value?.validate((valid) => {
    if (valid) {
      // Build payload with backend-expected field names
      const buildPayload = (src) => {
        const short = src.facShort ?? src.facCode
        return {
          // backend expects facName
          facName: src.facName,
          // backend uses facAddress / shortName (or shortName)
          facAddress: src.address,
          shortName: short,
          // also include aliases in case backend expects different keys
          facShort: short,
          address: src.address,
          facCode: src.facCode,
          contact: src.contact,
          phone: src.phone,
          status: src.status,
          remark: src.remark
        }
      }

      if (form.facId) {
        // update: merge original server object with updated fields so we don't drop server-side fields
        const mapped = buildPayload(form)
        // start from raw server response if available, otherwise include facId
        const base = originalFactory.value && typeof originalFactory.value === 'object' ? { ...originalFactory.value } : { facId: form.facId }
        const payload = {
          ...base,
          ...mapped,
          // ensure facId is present under common keys
          facId: form.facId,
          facID: form.facId
        }
        // explicitly ensure facAddress and facShort/shortName are present
        payload.facAddress = mapped.facAddress ?? mapped.address ?? base.facAddress ?? base.facAddress ?? payload.facAddress
        payload.facShort = mapped.facShort ?? mapped.shortName ?? base.facShort ?? base.shortName ?? payload.facShort
        // also provide shortName alias for compatibility
        payload.shortName = mapped.shortName ?? mapped.facShort ?? base.shortName ?? base.facShort ?? payload.shortName
        // remove client-only reactive fields that backend may not expect (optional)
        if (payload.params) delete payload.params
  // Provide PascalCase aliases in case backend expects different naming (C# property names)
  payload.FacId = payload.facId ?? payload.FacId
  payload.FacID = payload.facID ?? payload.FacID ?? payload.FacId
  payload.FacAddress = payload.facAddress ?? payload.FacAddress
  payload.FacShort = payload.facShort ?? payload.FacShort
  payload.ShortName = payload.shortName ?? payload.ShortName
        updateFactory(payload).then(() => {
          ElMessage.success(t('common.updateSuccess'))
          dialogVisible.value = false
          getList()
        })
      } else {
        // add: do not send facId
        const payload = buildPayload(form)
        addFactory(payload).then(() => {
          ElMessage.success(t('common.addSuccess'))
          dialogVisible.value = false
          getList()
        })
      }
    }
  })
}

const reset = () => {
  Object.assign(form, {
    facId: null,
    facName: '',
    facCode: '',
    facShort: '',
    address: '',
    contact: '',
    phone: '',
    status: '0',
    remark: ''
  })
  formRef.value?.resetFields()
}

// Lifecycle
onMounted(() => {
  getList()
})
</script>

<style scoped lang="scss">
.app-container {
  padding: 20px;
}
</style>
