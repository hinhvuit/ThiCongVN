import Layout from '@/layout'

const constructionRouter = {
  path: '/construction',
  component: Layout,
  redirect: '/construction/workers',
  name: 'Construction',
  meta: {
    title: 'Quản lý công trình',
    icon: 'tool'
  },
  children: [
    {
      path: 'workers',
      component: () => import('@/views/construction/workers/index.vue'),
      name: 'Workers',
      meta: {
        title: 'Quản lý nhân viên',
        icon: 'user'
      }
    }
    ,
    {
      path: 'factory',
      component: () => import('@/views/system/factory/index.vue'),
      name: 'Factory',
      meta: {
        title: 'Factory',
        icon: 'factory'
      }
    }
  ]
}

export default constructionRouter
