<template>
  <div class="app-container">
    <!-- Header Actions -->
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <area-select />
      </el-col>
      <el-col :span="1.5">
        <el-button type="primary" plain :icon="Plus" @click="handleAdd">
          {{ $t('btn.add') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="danger" plain :icon="Delete" :disabled="multiple" @click="handleDelete">
          {{ $t('btn.delete') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="warning" plain :icon="Download" @click="handleExport">
          {{ $t('btn.export') }}
        </el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <!-- Search Form -->
    <el-form v-show="showSearch" ref="queryRef" :model="queryParams" :inline="true">
      <el-form-item :label="$t('records.documentName')" prop="docName">
        <el-input v-model="queryParams.docName" :placeholder="$t('common.pleaseEnter')" clearable @keyup.enter="handleQuery" />
      </el-form-item>
      <el-form-item :label="$t('records.documentType')" prop="docTypeId">
        <el-select v-model="queryParams.docTypeId" :placeholder="$t('common.pleaseSelect')" clearable>
          <el-option v-for="item in documentTypes" :key="item.docTypeId" :label="item.docTypeName" :value="item.docTypeId" />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('records.uploadedBy')" prop="vendorName">
        <el-input v-model="queryParams.vendorName" :placeholder="$t('common.pleaseEnter')" clearable @keyup.enter="handleQuery" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" :icon="Search" @click="handleQuery">{{ $t('btn.search') }}</el-button>
        <el-button :icon="Refresh" @click="resetQuery">{{ $t('btn.reset') }}</el-button>
      </el-form-item>
    </el-form>

    <!-- Data Table -->
    <el-table v-loading="loading" :data="documentList" @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column :label="$t('records.documentName')" align="center" prop="docName" show-overflow-tooltip />
      <el-table-column :label="$t('records.documentType')" align="center" prop="documentTypeDto.docTypeName" show-overflow-tooltip />
      <el-table-column :label="$t('construction.factory')" align="center" prop="facId" show-overflow-tooltip>
        <template #default="scope">
          {{ getFactoryName(scope.row.facId) }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('records.uploadedBy')" align="center" prop="vendorName" show-overflow-tooltip />
      <el-table-column :label="$t('records.uploadDate')" align="center" prop="createTime" width="180">
        <template #default="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('common.remark')" align="center" prop="remark" show-overflow-tooltip />
      <el-table-column :label="$t('common.operation')" align="center" width="200">
        <template #default="scope">
          <el-button v-if="scope.row.viewUrl" link type="primary" :icon="View" @click="handleView(scope.row)">
            {{ $t('btn.view') }}
          </el-button>
          <el-button v-if="scope.row.downloadUrl" link type="primary" :icon="Download" @click="handleDownload(scope.row)">
            {{ $t('btn.download') }}
          </el-button>
          <el-button link type="primary" :icon="Edit" @click="handleUpdate(scope.row)">
            {{ $t('btn.edit') }}
          </el-button>
          <el-button link type="danger" :icon="Delete" @click="handleDelete(scope.row)">
            {{ $t('btn.delete') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- Pagination -->
    <pagination
      v-show="total > 0"
      v-model:page="queryParams.pageNum"
      v-model:limit="queryParams.pageSize"
      :total="total"
      @pagination="getList"
    />

    <!-- Add/Edit Dialog -->
    <el-dialog v-model="open" :title="title" width="600px" append-to-body>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="120px">
        <el-form-item :label="$t('records.documentName')" prop="docName">
          <el-input v-model="form.docName" :placeholder="$t('common.pleaseEnter')" />
        </el-form-item>
        <el-form-item :label="$t('records.documentType')" prop="documentTypeDto">
          <el-select v-model="form.documentTypeDto" :placeholder="$t('common.pleaseSelect')" style="width: 100%">
            <el-option v-for="item in documentTypes" :key="item.docTypeId" :label="item.docTypeName" :value="item" />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('construction.factory')" prop="facId">
          <el-select v-model="form.facId" :placeholder="$t('common.pleaseSelect')" style="width: 100%">
            <el-option v-for="factory in factories" :key="factory.facId" :label="factory.facName" :value="factory.facId" />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('records.uploadedBy')" prop="vendorName">
          <el-input v-model="form.vendorName" :placeholder="$t('common.pleaseEnter')" />
        </el-form-item>
        <el-form-item :label="$t('common.remark')" prop="remark">
          <el-input v-model="form.remark" type="textarea" :placeholder="$t('common.pleaseEnter')" />
        </el-form-item>
        <el-form-item v-if="form.docId && form.fileId" label="现有文件">
          <el-link :href="form.viewUrl" target="_blank">查看文件</el-link>
        </el-form-item>
        <el-form-item label="文件" prop="fileId">
          <file-upload v-model="form.fileId" :limit="1" />
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button type="primary" @click="submitForm">{{ $t('btn.confirm') }}</el-button>
          <el-button @click="cancel">{{ $t('btn.cancel') }}</el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="Document">
import { ref, reactive, computed, onMounted } from 'vue'
import useUserStore from '@/store/modules/user'
import { useI18n } from 'vue-i18n'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Delete, Download, Search, Refresh, View, Edit } from '@element-plus/icons-vue'
import { listDocument, getDocument, addDocument, updateDocument, delDocument, getDocumentTypes, exportDocument } from '@/api/records/document'
import { parseTime } from '@/utils/ruoyi'
import AreaSelect from '@/components/AreaSelect'

const { t } = useI18n()
const userStore = useUserStore()
const baseUrl = import.meta.env.VITE_APP_BASE_API

// Data
const documentList = ref([])
const documentTypes = ref([])
const loading = ref(true)
const showSearch = ref(true)
const ids = ref([])
const single = ref(true)
const multiple = ref(true)
const total = ref(0)
const title = ref('')
const open = ref(false)

// Query params
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  docName: null,
  docTypeId: null,
  vendorName: null,
  facId: null
})

// Form
const form = ref({})
const formRef = ref(null)
const queryRef = ref(null)

// Rules
const rules = {
  docName: [{ required: true, message: t('records.pleaseEnterDocumentName'), trigger: 'blur' }],
  documentTypeDto: [{ required: true, message: t('common.pleaseSelect'), trigger: 'change' }],
  facId: [{ required: true, message: t('common.pleaseSelect'), trigger: 'change' }]
}

// Computed
const factories = computed(() => userStore.factories || [])

// Methods
const getList = () => {
  loading.value = true
  // Get selected area from localStorage
  const selectedAreaId = localStorage.getItem('selectedAreaId')
  if (selectedAreaId) {
    queryParams.facId = parseInt(selectedAreaId)
  }
  
  listDocument(queryParams).then(response => {
    documentList.value = response.rows || []
    total.value = response.total || 0
    loading.value = false
  }).catch(() => {
    documentList.value = []
    total.value = 0
    loading.value = false
  })
}

const getFactoryName = (facId) => {
  const factory = factories.value.find(f => f.facId === facId)
  return factory ? factory.facName : facId
}

const handleQuery = () => {
  queryParams.pageNum = 1
  getList()
}

const resetQuery = () => {
  queryRef.value?.resetFields()
  handleQuery()
}

const handleSelectionChange = (selection) => {
  ids.value = selection.map(item => item.docId)
  single.value = selection.length !== 1
  multiple.value = !selection.length
}

const handleAdd = () => {
  reset()
  // Set default factory from selected area
  const selectedAreaId = localStorage.getItem('selectedAreaId')
  if (selectedAreaId) {
    form.value.facId = parseInt(selectedAreaId)
  }
  open.value = true
  title.value = t('records.addDocument')
}

const handleUpdate = (row) => {
  reset()
  const docId = row.docId || ids.value[0]
  getDocument(docId).then(response => {
    form.value = response.data
    // If backend returned file info, put a combined string into form.fileId so FileUpload can show original name
    if (response.data && response.data.fileId) {
      const fid = response.data.fileId
      const oldName = response.data.oldName || ''
      const view = response.data.viewUrl || `${baseUrl}/system/file/view/${fid}`
      form.value.fileId = `${fid}|${oldName}|${view}`
      // keep form.viewUrl for direct links elsewhere
      form.value.viewUrl = view
    }
    open.value = true
    title.value = t('records.editDocument')
  })
}

const handleDelete = (row) => {
  const docIds = row.docId ? [row.docId] : ids.value
  ElMessageBox.confirm(
    t('common.confirmDelete'),
    t('common.tips'),
    {
      confirmButtonText: t('btn.confirm'),
      cancelButtonText: t('btn.cancel'),
      type: 'warning'
    }
  ).then(() => {
    return delDocument(docIds.join(','))
  }).then(() => {
    getList()
    ElMessage.success(t('common.deleteSuccess'))
  }).catch(() => {})
}

const handleView = (row) => {
  if (row.viewUrl) {
    window.open(row.viewUrl, '_blank')
  }
}

const handleDownload = (row) => {
  if (row.downloadUrl) {
    window.open(row.downloadUrl, '_blank')
  }
}

const handleExport = () => {
  exportDocument(queryParams).then(response => {
    ElMessage.success(t('common.success'))
  })
}

const submitForm = () => {
  formRef.value?.validate(valid => {
    if (valid) {
      if (form.value.docId) {
        if (!form.value.fileId || form.value.fileId === '') {
          form.value.fileId = null
        }
        updateDocument(form.value).then(() => {
          ElMessage.success(t('common.updateSuccess'))
          open.value = false
          getList()
        })
      } else {
        const addData = { ...form.value }
        delete addData.docId
        if (!addData.fileId || addData.fileId === '') {
          delete addData.fileId
        }
        addDocument(addData).then(() => {
          ElMessage.success(t('common.addSuccess'))
          open.value = false
          getList()
        })
      }
    }
  })
}

const cancel = () => {
  open.value = false
  reset()
}

const reset = () => {
  form.value = {
    docId: null,
    docName: null,
    documentTypeDto: null,
    facId: null,
    vendorName: null,
    fileId: null,
    remark: null
  }
  formRef.value?.resetFields()
}

const loadDocumentTypes = () => {
  getDocumentTypes().then(response => {
    documentTypes.value = response.data || []
  })
}

// Lifecycle
onMounted(() => {
  getList()
  loadDocumentTypes()
})
</script>
