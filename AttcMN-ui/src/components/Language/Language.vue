<template>
    <el-dropdown @command="handleSetLanguage" class="hover-effect"> 
      <span>
        {{ selectedLanguage }}
        <el-icon class="el-icon--right">
          <arrow-down />
        </el-icon>
      </span>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item command="vn" :disabled="language==='vn'">Tiếng Việt</el-dropdown-item>
          <el-dropdown-item command="zh" :disabled="language==='zh'">简体中文</el-dropdown-item>
          <el-dropdown-item command="en" :disabled="language==='en'">English</el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { ArrowDown } from '@element-plus/icons-vue'

const { locale } = useI18n()
const language = ref(localStorage.getItem('locale') || 'vn')

const selectedLanguage = computed(() => {
  if (language.value === 'vn') return 'Tiếng Việt'
  if (language.value === 'zh') return '简体中文'
  if (language.value === 'en') return 'English'
  return 'Tiếng Việt'
})

const handleSetLanguage = (newLang) => {
  console.log('Switching language to:', newLang)
  language.value = newLang
  locale.value = newLang
  localStorage.setItem('locale', newLang)
  setTimeout(() => {
    location.reload()
  }, 300)
}

onMounted(() => {
  console.log('Current language:', language.value)
  if (!localStorage.getItem('locale')) {
    const langArr = ['zh', 'en', 'vn']
    const navLang = navigator.language.substring(0, 2)
    const defaultLang = langArr.indexOf(navLang) === -1 ? 'vn' : navLang
    localStorage.setItem('locale', defaultLang)
    language.value = defaultLang
    locale.value = defaultLang
  } else {
    // Ensure i18n locale matches localStorage
    locale.value = language.value
  }
})
</script>

<style lang="scss" scoped>
.hover-effect {
  cursor: pointer;
  transition: background 0.3s;

  &:hover {
    background: rgba(0, 0, 0, 0.025);
  }
}
.hover-effect:hover {
  background: none !important;
}
:deep(.el-tooltip__trigger:focus-visible) {
  outline: unset;
}
</style>