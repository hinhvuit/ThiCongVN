<template>
  <div v-if="!item.hidden">
    <template v-if="hasOneShowingChild(item.children, item) && (!onlyOneChild.children || onlyOneChild.noShowingChildren) && !item.alwaysShow">
      <app-link v-if="onlyOneChild.meta" :to="resolvePath(onlyOneChild.path, onlyOneChild.query)">
        <el-menu-item :index="resolvePath(onlyOneChild.path)" :class="{ 'submenu-title-noDropdown': !isNest }">
          <svg-icon :icon-class="onlyOneChild.meta.icon || (item.meta && item.meta.icon)"/>
          <template #title>
            <span class="menu-title" :title="hasTitle(onlyOneChild.meta.title)">
              {{ translatedChildTitle }}
            </span>
          </template>
        </el-menu-item>
      </app-link>
    </template>

    <el-sub-menu v-else ref="subMenu" :index="resolvePath(item.path)" teleported>
      <template v-if="item.meta" #title>
        <svg-icon :icon-class="item.meta && item.meta.icon" />
        <span class="menu-title" :title="hasTitle(item.meta.title)">
          {{ translatedItemTitle }}
        </span>
      </template>

      <sidebar-item
        v-for="(child, index) in item.children"
        :key="child.path + index"
        :is-nest="true"
        :item="child"
        :base-path="resolvePath(child.path)"
        class="nest-menu"
      />
    </el-sub-menu>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { isExternal } from '@/utils/validate'
import AppLink from './Link'
import { getNormalPath } from '@/utils/ruoyi'
import { useI18n } from 'vue-i18n'

const { t, locale } = useI18n()

const props = defineProps({
  // route object
  item: {
    type: Object,
    required: true
  },
  isNest: {
    type: Boolean,
    default: false
  },
  basePath: {
    type: String,
    default: ''
  }
})

const onlyOneChild = ref({});

// Chinese to translation key map
const chineseToKeyMap = {
  '首页': 'route.dashboard',
  '个人中心': 'route.profile',
  '系统管理': 'route.system',
  '用户管理': 'route.user',
  '角色管理': 'route.role',
  '菜单管理': 'route.menu',
  '部门管理': 'route.dept',
  '岗位管理': 'route.post',
  '字典管理': 'route.dict',
  '参数设置': 'route.config',
  '通知公告': 'route.notice',
  '日志管理': 'route.log',
  '系统工具': 'route.tool',
  '系统监控': 'route.monitor',
  '在线用户': 'route.online',
  '定时任务': 'route.job',
  '数据监控': 'route.druid',
  '服务监控': 'route.server',
  '缓存监控': 'route.cache',
  '施工管理': 'route.construction',
  '工人管理': 'route.workers',
  '承包商管理': 'route.contractors',
  '区域管理': 'route.areas',
  '作业许可证': 'route.workPermits',
  '档案管理': 'route.records',
  '文档管理': 'route.documents',
  '进度管理': 'route.progress',
  '工厂管理': 'route.factory',
  '分配角色': 'route.authRole',
  '分配用户': 'route.authUser',
  '字典数据': 'route.dictData',
  '调度日志': 'route.jobLog',
  '修改生成配置': 'route.genEdit',
}

// Computed properties for translated titles - these are reactive to locale changes
const translatedChildTitle = computed(() => {
  const meta = onlyOneChild.value?.meta
  return translateMeta(meta)
})

const translatedItemTitle = computed(() => {
  return translateMeta(props.item?.meta)
})

// Translate function
function translateMeta(meta) {
  if (!meta || !meta.title) return ''
  
  // Access locale.value to track reactivity
  const _ = locale.value
  
  // If titleKey is provided (from permission store), use it directly
  if (meta.titleKey) {
    const translated = t(meta.titleKey)
    if (translated !== meta.titleKey) {
      return translated
    }
  }
  
  // Try multiple translation key patterns
  const patterns = [
    `route.${meta.title}`,
    `menu.${meta.title}`,
    meta.title,
  ]
  
  for (const key of patterns) {
    const translated = t(key)
    if (translated !== key) {
      return translated
    }
  }
  
  // If backend returns Chinese text, try to map it
  if (chineseToKeyMap[meta.title]) {
    const translated = t(chineseToKeyMap[meta.title])
    if (translated !== chineseToKeyMap[meta.title]) {
      return translated
    }
  }
  
  // Return original title if no translation found
  return meta.title
}

function hasOneShowingChild(children = [], parent) {
  if (!children) {
    children = [];
  }
  const showingChildren = children.filter(item => {
    if (item.hidden) {
      return false
    } else {
      // Temp set(will be used if only has one showing child)
      onlyOneChild.value = item
      return true
    }
  })

  // When there is only one child router, the child router is displayed by default
  if (showingChildren.length === 1) {
    return true
  }

  // Show parent if there are no child router to display
  if (showingChildren.length === 0) {
    onlyOneChild.value = { ...parent, path: '', noShowingChildren: true }
    return true
  }

  return false
};

function resolvePath(routePath, routeQuery) {
  if (isExternal(routePath)) {
    return routePath
  }
  if (isExternal(props.basePath)) {
    return props.basePath
  }
  if (routeQuery) {
    let query = JSON.parse(routeQuery);
    return { path: getNormalPath(props.basePath + '/' + routePath), query: query }
  }
  return getNormalPath(props.basePath + '/' + routePath)
}

function hasTitle(title){
  if (title && title.length > 5) {
    return title;
  } else {
    return "";
  }
}
</script>
