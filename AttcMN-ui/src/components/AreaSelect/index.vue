<template>
  <el-dropdown trigger="click" @command="handleSetArea" class="area-select-dropdown">
    <div class="area-select-trigger">
      <span class="area-text">{{ currentAreaName }}</span>
      <el-icon class="arrow-icon"><ArrowDown /></el-icon>
    </div>
    <template #dropdown>
      <el-dropdown-menu>
        <el-dropdown-item 
          v-for="area in areaList" 
          :key="area.facId" 
          :command="area"
          :disabled="currentAreaId === area.facId"
        >
          <span :style="{ color: currentAreaId === area.facId ? '#409EFF' : '' }">
            {{ area.facName }}
          </span>
          <el-icon v-if="currentAreaId === area.facId" style="margin-left: 10px; color: #409EFF;">
            <Check />
          </el-icon>
        </el-dropdown-item>
      </el-dropdown-menu>
    </template>
  </el-dropdown>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { Check, ArrowDown } from '@element-plus/icons-vue'
import { useI18n } from 'vue-i18n'
import useUserStore from '@/store/modules/user'

const { t } = useI18n()
const userStore = useUserStore()

const currentAreaId = ref(null)
const currentAreaName = ref(t('navbar.selectArea') || 'Select Area')

// Computed
const areaList = computed(() => {
  return userStore.factories || []
})

// Methods
const initializeArea = () => {
  if (!areaList.value || areaList.value.length === 0) return
  
  // Load saved area from localStorage
  const savedAreaId = localStorage.getItem('selectedAreaId')
  const savedAreaName = localStorage.getItem('selectedAreaName')
  
  if (savedAreaId && savedAreaName) {
    // Validate saved area is still in user's permission list
    const areaExists = areaList.value.find(a => a.facId === parseInt(savedAreaId))
    if (areaExists) {
      currentAreaId.value = parseInt(savedAreaId)
      currentAreaName.value = savedAreaName
      userStore.setSelectedArea(areaExists)
      return
    }
  }
  
  // If no valid saved area, select first one
  if (areaList.value.length > 0) {
    handleSetArea(areaList.value[0])
  }
}

const handleSetArea = (area) => {
  currentAreaId.value = area.facId
  currentAreaName.value = area.facName
  
  // Save to localStorage
  localStorage.setItem('selectedAreaId', area.facId)
  localStorage.setItem('selectedAreaName', area.facName)
  
  // Emit event for parent components
  userStore.setSelectedArea(area)
  
  // Notify user
  ElMessage.success(`${t('navbar.areaChanged') || 'Area changed to'}: ${area.facName}`)
}

// Watch
watch(() => userStore.factories, (newVal) => {
  if (newVal && newVal.length > 0) {
    initializeArea()
  }
}, { immediate: true })

// Lifecycle
onMounted(() => {
  initializeArea()
})
</script>

<style scoped>
.area-select-dropdown {
  display: inline-flex;
  align-items: center;
  height: 100%;
  cursor: pointer;
  margin-right: 20px;
}

.area-select-trigger {
  display: flex;
  align-items: center;
  padding: 0 12px;
  height: 35px;
  border: 1px solid #dcdfe6;
  border-radius: 4px;
  background-color: #fff;
  transition: border-color 0.2s;
}

.area-select-trigger:hover {
  border-color: #c0c4cc;
}

.area-text {
  font-size: 14px;
  color: #606266;
  margin-right: 20px;
  white-space: nowrap;
}

.arrow-icon {
  font-size: 12px;
  color: #909399;
  transition: transform 0.3s;
}

.area-select-dropdown:hover .arrow-icon {
  color: #606266;
}
</style>
