import Layout from '@/layout'

/**
 * 📂 HỒ SƠ (RECORDS MODULE)
 * Quản lý tài liệu và tiến độ công trình
 */
const recordsRouter = {
  path: '/records',
  component: Layout,
  redirect: '/records/document',
  name: 'Records',
  meta: { 
    title: 'Hồ sơ',
    icon: 'documentation' 
  },
  children: [
    {
      path: 'document',
      component: () => import('@/views/records/documents/index.vue'),
      name: 'Documents',
      meta: { 
        title: 'Tài liệu công trình',
        icon: 'documentation',
        noCache: true
      }
    }
    // TODO: Thêm route progress khi đã tạo view
    // {
    //   path: 'progress',
    //   component: () => import('@/views/records/progress/index'),
    //   name: 'Progress',
    //   meta: { 
    //     title: 'Tiến độ thi công',
    //     icon: 'chart',
    //     noCache: true
    //   }
    // }
  ]
}

export default recordsRouter
