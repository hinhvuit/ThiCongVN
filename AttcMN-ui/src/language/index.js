import { createI18n } from 'vue-i18n';
import pinia from '@/store/index';
import { storeToRefs } from 'pinia';
import useSettingsStore from '@/store/modules/settings';

// element plus 自带国际化
import enLocale from 'element-plus/es/locale/lang/en';
import zhcnLocale from 'element-plus/es/locale/lang/zh-cn';
import viLocale from 'element-plus/es/locale/lang/vi';
// 引入自定义中文包
import customZH from './locales/zh'
// 引入自定义英文包
import customEN from './locales/en'
// 引入自定义越南语包
import customVN from './locales/vn'

// 读取 localStorage 中的语言设置，优先使用用户选择的语言
const lang = localStorage.getItem('locale') || 'vn';

// 同步到 store
const stores = useSettingsStore(pinia);
const settingsStore = storeToRefs(stores);
if (settingsStore.globalI18n.value !== lang) {
  stores.setGlobalI18n(lang);
}

// import.meta.glob  vite 中用来批量导入模块的方法
const modules = import.meta.glob('./**/*.js', { eager: true });
const itemize = { 'zh': [], 'en': [], 'vn': [] };
const element = { 'zh': zhcnLocale, 'en': enLocale, 'vn': viLocale };
const messages = {};

// // 对自动引入的 modules 进行分类 zh、en
// for (const path in modules) {
//   const flag = path.match(/(\S+)\/(\S+).js/);
//   const key = flag[flag.length - 1];
//   if (itemize[key]) itemize[key].push(modules[path].default);
//   else itemize[key] = modules[path];
// }

// // 合并数组对象（非标准数组对象，数组中对象的每项 key、value 都不同）
// function mergeArrObj(list, key) {
//   let obj = {};
//   list[key].forEach((i) => {
//     obj = Object.assign({}, obj, i);
//   });
//   return obj;
// }

// // 处理最终格式
// for (const key in itemize) {
//   messages[key] = {
//     name: key,
//     el: element[key].el,
//     message: mergeArrObj(itemize, key)
//   };
// }

export const i18n = createI18n({
  legacy: false,
  locale: lang, // 默认显示语言
  globalInjection: true, // 全局注入 $t 函数
  silentTranslationWarn: true, // 去掉警告
  missingWarn: false,
  silentFallbackWarn: true, //抑制警告
  fallbackLocale: 'vn',
  messages: {
    // 英文环境下的语言数据
    en: {
      ...enLocale,
      ...customEN
    },
    // 中文环境下的语言数据
    zh: {
      ...zhcnLocale,
      ...customZH
    },
    // 越南语环境下的语言数据
    vn: {
      ...viLocale,
      ...customVN
    }
  }
});

const translate = (localeKey) => {
  const locale = localStorage.getItem('locale') || "vn"
  console.log('i18n',i18n);
  
  const hasKey = i18n.global.te(localeKey, locale); // 使用i18n的 te 方法来检查是否能够匹配到对应键值
  const translatedStr = i18n.global.t(localeKey);
  if (hasKey) {
    return translatedStr;
  }
  return localeKey;
};

export default i18n
export { translate }