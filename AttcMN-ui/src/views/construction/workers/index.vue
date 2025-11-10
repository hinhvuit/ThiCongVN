<template>
  <div class="app-container">
    <!-- Search Form -->
    <el-form :model="queryParams" ref="queryFormRef" :inline="true" v-show="showSearch" label-width="100px">
      <el-form-item :label="$t('construction.contractor')" prop="contractorId">
        <el-select v-model="queryParams.contractorId" :placeholder="$t('common.pleaseSelect')" clearable style="width: 200px">
          <el-option
            v-for="contractor in contractorOptions"
            :key="contractor.id"
            :label="contractor.name"
            :value="contractor.id"
          />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('construction.area')" prop="areaId">
        <el-select v-model="queryParams.areaId" :placeholder="$t('common.pleaseSelect')" clearable style="width: 200px">
          <el-option
            v-for="area in areaOptions"
            :key="area.id"
            :label="area.name"
            :value="area.id"
          />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('construction.workerName')" prop="workerName">
        <el-input
          v-model="queryParams.workerName"
          :placeholder="$t('construction.pleaseEnterWorkerName')"
          clearable
          style="width: 200px"
          @keyup.enter="handleQuery"
        />
      </el-form-item>
      <el-form-item :label="$t('construction.idCard')" prop="idCard">
        <el-input
          v-model="queryParams.idCard"
          :placeholder="$t('construction.pleaseEnterIdCard')"
          clearable
          style="width: 200px"
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
      <el-col :span="1.5">
        <el-button type="warning" plain :icon="Download" @click="handleExport">{{ $t('common.export') }}</el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="info" plain :icon="Upload" @click="handleImport">{{ $t('common.import') }}</el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <!-- Data Table -->
    <el-table v-loading="loading" :data="workerList" @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column label="STT" type="index" width="50" align="center" />
      <el-table-column :label="$t('construction.workerId')" align="center" prop="workId" width="150" show-overflow-tooltip />
      <el-table-column :label="$t('construction.contractor')" align="center" prop="contractorName" width="150" show-overflow-tooltip />
      <el-table-column :label="$t('construction.workerName')" align="center" prop="workerName" width="120" />
      <el-table-column :label="$t('construction.idCard')" align="center" prop="idCard" width="180" />
      <el-table-column :label="$t('construction.accidentInsurance')" align="center" prop="accidentInsurance" width="120">
        <template #default="scope">
          <el-tag v-if="scope.row.accidentInsurance" type="success">{{ $t('common.yes') }}</el-tag>
          <el-tag v-else type="danger">{{ $t('common.no') }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column :label="$t('construction.professionalCert')" align="center" prop="professionalCert" width="150" show-overflow-tooltip />
      <el-table-column :label="$t('construction.equipmentCert')" align="center" prop="equipmentCert" width="150" show-overflow-tooltip />
      
      <!-- Safety Cards Group -->
      <el-table-column :label="$t('construction.safetyCards')" align="center">
        <el-table-column :label="$t('construction.safetyCard2')" align="center" width="120">
          <template #default="scope">
            <el-tag v-if="scope.row.safetyCard2" type="success" size="small">
              {{ scope.row.safetyCard2 }}
            </el-tag>
            <el-tag v-else type="info" size="small">-</el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="$t('construction.safetyCard2Expiry')" align="center" prop="safetyCard2Expiry" width="120">
          <template #default="scope">
            <span v-if="scope.row.safetyCard2Expiry" :class="getExpiryClass(scope.row.safetyCard2Expiry)">
              {{ scope.row.safetyCard2Expiry }}
            </span>
          </template>
        </el-table-column>
        
        <el-table-column :label="$t('construction.safetyCard3')" align="center" width="120">
          <template #default="scope">
            <el-tag v-if="scope.row.safetyCard3" type="success" size="small">
              {{ scope.row.safetyCard3 }}
            </el-tag>
            <el-tag v-else type="info" size="small">-</el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="$t('construction.safetyCard3Expiry')" align="center" prop="safetyCard3Expiry" width="120">
          <template #default="scope">
            <span v-if="scope.row.safetyCard3Expiry" :class="getExpiryClass(scope.row.safetyCard3Expiry)">
              {{ scope.row.safetyCard3Expiry }}
            </span>
          </template>
        </el-table-column>
        
        <el-table-column :label="$t('construction.safetyCard4')" align="center" width="120">
          <template #default="scope">
            <el-tag v-if="scope.row.safetyCard4" type="success" size="small">
              {{ scope.row.safetyCard4 }}
            </el-tag>
            <el-tag v-else type="info" size="small">-</el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="$t('construction.safetyCard4Expiry')" align="center" prop="safetyCard4Expiry" width="120">
          <template #default="scope">
            <span v-if="scope.row.safetyCard4Expiry" :class="getExpiryClass(scope.row.safetyCard4Expiry)">
              {{ scope.row.safetyCard4Expiry }}
            </span>
          </template>
        </el-table-column>
        <el-table-column :label="$t('construction.safetyCard4EntryDate')" align="center" prop="safetyCard4EntryDate" width="120" />
      </el-table-column>
      
      <el-table-column :label="$t('construction.electricalSafetyCard')" align="center" prop="electricalSafetyCard" width="150" />
      <el-table-column :label="$t('construction.healthCertificate')" align="center" prop="healthCertificate" width="150" />
      <el-table-column :label="$t('construction.workerPhoto')" align="center" width="100">
        <template #default="scope">
          <el-image
            v-if="scope.row.workerPhoto"
            style="width: 50px; height: 50px"
            :src="scope.row.workerPhoto"
            :preview-src-list="[scope.row.workerPhoto]"
            fit="cover"
          />
        </template>
      </el-table-column>
      
      <el-table-column :label="$t('common.actions')" align="center" width="200" fixed="right">
        <template #default="scope">
          <el-button link type="primary" :icon="View" @click="handleView(scope.row)">{{ $t('common.view') }}</el-button>
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
    <el-dialog :title="dialogTitle" v-model="dialogVisible" width="800px" append-to-body>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="150px">
        <el-row>
          <el-col :span="12">
            <el-form-item :label="$t('construction.workerName')" prop="workerName">
              <el-input v-model="form.workerName" :placeholder="$t('construction.pleaseEnterWorkerName')" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('construction.idCard')" prop="idCard">
              <el-input v-model="form.idCard" :placeholder="$t('construction.pleaseEnterIdCard')" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item :label="$t('construction.contractor')" prop="contractorId">
              <el-select v-model="form.contractorId" :placeholder="$t('common.pleaseSelect')" style="width: 100%">
                <el-option
                  v-for="contractor in contractorOptions"
                  :key="contractor.id"
                  :label="contractor.name"
                  :value="contractor.id"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('construction.area')" prop="areaId">
              <el-select v-model="form.areaId" :placeholder="$t('common.pleaseSelect')" style="width: 100%">
                <el-option
                  v-for="area in areaOptions"
                  :key="area.id"
                  :label="area.name"
                  :value="area.id"
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item :label="$t('construction.accidentInsurance')" prop="accidentInsurance">
              <el-switch v-model="form.accidentInsurance" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('construction.professionalCert')" prop="professionalCert">
              <el-input v-model="form.professionalCert" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item :label="$t('common.remark')" prop="remark">
          <el-input v-model="form.remark" type="textarea" :rows="3" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">{{ $t('common.cancel') }}</el-button>
        <el-button type="primary" @click="submitForm">{{ $t('common.confirm') }}</el-button>
      </template>
    </el-dialog>

    <!-- Import Dialog -->
    <el-dialog :title="$t('common.import')" v-model="importDialogVisible" width="400px" append-to-body>
      <el-upload
        ref="uploadRef"
        :action="uploadUrl"
        :headers="uploadHeaders"
        :on-success="handleImportSuccess"
        :on-error="handleImportError"
        :limit="1"
        accept=".xlsx, .xls"
        :auto-upload="false"
      >
        <template #trigger>
          <el-button type="primary">{{ $t('common.selectFile') }}</el-button>
        </template>
        <template #tip>
          <div class="el-upload__tip">
            <el-button type="text" @click="handleDownloadTemplate">{{ $t('common.downloadTemplate') }}</el-button>
          </div>
        </template>
      </el-upload>
      <template #footer>
        <el-button @click="importDialogVisible = false">{{ $t('common.cancel') }}</el-button>
        <el-button type="primary" @click="submitImport">{{ $t('common.confirm') }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="Workers">
import { ref, reactive, computed, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Search, Refresh, Plus, Edit, Delete, Download, Upload, View } from '@element-plus/icons-vue'
import { useI18n } from 'vue-i18n'
import { getToken } from '@/utils/auth'
import { 
  listWorkers, 
  getWorker, 
  addWorker, 
  updateWorker, 
  delWorker,
  delWorkers,
  exportWorkers,
  importWorkers,
  downloadTemplate
} from '@/api/construction/workers'

const { t } = useI18n()

// State
const loading = ref(false)
const showSearch = ref(true)
const workerList = ref([])
const contractorOptions = ref([])
const areaOptions = ref([])
const total = ref(0)
const dialogVisible = ref(false)
const importDialogVisible = ref(false)
const dialogTitle = ref('')
const single = ref(true)
const multiple = ref(true)
const ids = ref([])

// Refs
const queryFormRef = ref()
const formRef = ref()
const uploadRef = ref()

// Query params
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  contractorId: null,
  areaId: null,
  workerName: null,
  idCard: null
})

// Form data
const form = reactive({
  workId: null,
  workerName: '',
  idCard: '',
  contractorId: null,
  areaId: null,
  accidentInsurance: false,
  professionalCert: '',
  remark: ''
})

// Form validation rules
const rules = {
  workerName: [{ required: true, message: t('construction.pleaseEnterWorkerName'), trigger: 'blur' }],
  idCard: [{ required: true, message: t('construction.pleaseEnterIdCard'), trigger: 'blur' }],
  contractorId: [{ required: true, message: t('common.pleaseSelect'), trigger: 'change' }],
  areaId: [{ required: true, message: t('common.pleaseSelect'), trigger: 'change' }]
}

// Upload configuration
const uploadUrl = computed(() => import.meta.env.VITE_APP_BASE_API + '/construction/workers/import')
const uploadHeaders = computed(() => ({ Authorization: 'Bearer ' + getToken() }))

// Methods
const getList = () => {
  loading.value = true
  listWorkers(queryParams).then(response => {
    workerList.value = response.rows
    total.value = response.total
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
  ids.value = selection.map(item => item.workId)
  single.value = selection.length !== 1
  multiple.value = !selection.length
}

const handleAdd = () => {
  reset()
  dialogVisible.value = true
  dialogTitle.value = t('common.add') + ' ' + t('construction.worker')
}

const handleUpdate = (row) => {
  reset()
  const workId = row.workId || ids.value[0]
  getWorker(workId).then(response => {
    Object.assign(form, response.data)
    dialogVisible.value = true
    dialogTitle.value = t('common.edit') + ' ' + t('construction.worker')
  })
}

const handleView = (row) => {
  reset()
  getWorker(row.workId).then(response => {
    Object.assign(form, response.data)
    dialogVisible.value = true
    dialogTitle.value = t('common.view') + ' ' + t('construction.worker')
  })
}

const handleDelete = (row) => {
  const workIds = row.workId ? [row.workId] : ids.value
  ElMessageBox.confirm(
    t('common.confirmDelete'),
    t('common.tips'),
    {
      confirmButtonText: t('btn.confirm'),
      cancelButtonText: t('btn.cancel'),
      type: 'warning'
    }
  ).then(() => {
    return delWorkers(workIds.join(','))
  }).then(() => {
    getList()
    ElMessage.success(t('common.deleteSuccess'))
  }).catch(() => {})
}

const handleExport = () => {
  ElMessageBox.confirm(
    t('common.confirmExport'),
    t('common.tips'),
    {
      confirmButtonText: t('btn.confirm'),
      cancelButtonText: t('btn.cancel'),
      type: 'warning'
    }
  ).then(() => {
    return exportWorkers(queryParams)
  }).then(() => {
    ElMessage.success(t('common.exportSuccess'))
  }).catch(() => {})
}

const handleImport = () => {
  importDialogVisible.value = true
}

const submitImport = () => {
  uploadRef.value?.submit()
}

const handleImportSuccess = (response) => {
  importDialogVisible.value = false
  ElMessage.success(t('common.importSuccess'))
  getList()
}

const handleImportError = () => {
  ElMessage.error(t('common.importFailed'))
}

const handleDownloadTemplate = () => {
  downloadTemplate().then(() => {
    ElMessage.success(t('common.downloadSuccess'))
  })
}

// Get expiry date class for styling
const getExpiryClass = (expiryDate) => {
  if (!expiryDate) return ''
  const today = new Date()
  const expiry = new Date(expiryDate)
  const diffDays = Math.ceil((expiry - today) / (1000 * 60 * 60 * 24))
  
  if (diffDays < 0) return 'text-danger' // Expired
  if (diffDays <= 30) return 'text-warning' // Expiring soon (within 30 days)
  return 'text-success' // Valid
}

const submitForm = () => {
  formRef.value?.validate((valid) => {
    if (valid) {
      if (form.workId) {
        updateWorker(form).then(() => {
          ElMessage.success(t('common.updateSuccess'))
          dialogVisible.value = false
          getList()
        })
      } else {
        addWorker(form).then(() => {
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
    workId: null,
    workerName: '',
    idCard: '',
    contractorId: null,
    areaId: null,
    accidentInsurance: false,
    professionalCert: '',
    remark: ''
  })
  formRef.value?.resetFields()
}

// Load contractors and areas options
const loadOptions = () => {
  // TODO: Load from API when available
  // getAllContractors().then(response => { contractorOptions.value = response.data })
  // getUserAreas().then(response => { areaOptions.value = response.data })
}

// Lifecycle
onMounted(() => {
  getList()
  loadOptions()
})
</script>

<style scoped lang="scss">
.app-container {
  padding: 20px;
}

:deep(.text-danger) {
  color: #f56c6c;
  font-weight: bold;
}

:deep(.text-warning) {
  color: #e6a23c;
  font-weight: bold;
}

:deep(.text-success) {
  color: #67c23a;
}
</style>
