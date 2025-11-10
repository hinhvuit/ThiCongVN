<template>
  <el-config-provider :locale="getGlobalI18n"> <router-view /> </el-config-provider>
</template>

<script setup>
import { useI18n } from 'vue-i18n';
import useSettingsStore from '@/store/modules/settings'
import { handleThemeStyle } from '@/utils/theme'
// 定义变量内容
const { messages, locale } = useI18n();

onMounted(() => {
  nextTick(() => {
    // 初始化主题样式
    handleThemeStyle(useSettingsStore().theme)
  })
})
// 获取全局 i18n
const getGlobalI18n = computed(() => {
  return messages.value[locale.value];
});
</script>
<style>
@import url("./assets/css/public.css");

</style>
