import defaultSettings from '@/settings'
import { useDynamicTitle } from '@/utils/dynamicTitle'

const { sideTheme, showSettings, topNav, tagsView, fixedHeader, sidebarLogo, dynamicTitle,globalI18n } = defaultSettings

const storageSetting = JSON.parse(localStorage.getItem('ssp-yxht-layout-setting')) || ''
const langSetting = localStorage.getItem('locale') || ''

const useSettingsStore = defineStore(
  'settings',
  {
    state: () => ({
      title: '',
      theme: storageSetting.theme || '#1890FF',
      sideTheme: storageSetting.sideTheme || sideTheme,
      showSettings: showSettings,
      topNav: storageSetting.topNav === undefined ? topNav : storageSetting.topNav,
      tagsView: storageSetting.tagsView === undefined ? tagsView : storageSetting.tagsView,
      fixedHeader: storageSetting.fixedHeader === undefined ? fixedHeader : storageSetting.fixedHeader,
      sidebarLogo: storageSetting.sidebarLogo === undefined ? sidebarLogo : storageSetting.sidebarLogo,
      dynamicTitle: storageSetting.dynamicTitle === undefined ? dynamicTitle : storageSetting.dynamicTitle,
      globalI18n: langSetting ? langSetting : globalI18n,
    }),
    actions: {
      // 修改布局设置
      changeSetting(data) {
        const { key, value } = data
        if (this.hasOwnProperty(key)) {
          this[key] = value
        }
      },
      // 设置网页标题
      setTitle(title) {
        this.title = title
        useDynamicTitle();
      },
      // 设置多语言
      setGlobalI18n(lang){
        this.globalI18n = lang
      }      
    }
  })

export default useSettingsStore
