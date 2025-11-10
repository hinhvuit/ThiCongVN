// Menu translation mapping - Maps Chinese menu names to i18n keys
export const menuTranslationMap = {
  // System Management
  '系统管理': 'route.system',
  '用户管理': 'route.user',
  '角色管理': 'route.role',
  '菜单管理': 'route.menu',
  '部门管理': 'route.dept',
  '岗位管理': 'route.post',
  '字典管理': 'route.dict',
  '参数设置': 'route.config',
  '通知公告': 'route.notice',
  
  // Construction Management
  '施工管理': 'route.construction',
  '工人管理': 'route.workers',
  '承包商管理': 'route.contractors',
  '区域管理': 'route.areas',
  '作业许可': 'route.workPermits',
  '文档管理': 'route.documents',
  '进度管理': 'route.progress',
  
  // Records
  '档案管理': 'route.records',
  
  // System Tools
  '系统工具': 'route.systemTools',
  '表单构建': 'route.formBuilding',
  '代码生成': 'route.codeGeneration',
  '系统接口': 'route.systemInterface',
  
  // Monitoring
  '系统监控': 'route.monitor',
  '在线用户': 'route.online',
  '定时任务': 'route.job',
  '数据监控': 'route.druid',
  '服务监控': 'route.server',
  '缓存监控': 'route.cache',
  
  // Logs
  '日志管理': 'route.log',
  '操作日志': 'route.operlog',
  '登录日志': 'route.logininfor',
  
  // Dashboard
  '首页': 'route.dashboard',
  '个人中心': 'route.profile',
  
  // Factory
  '工厂管理': 'route.factory',
  
  // Analytics
  '数据分析': 'route.analytics',
  
  // Additional menus from backend
  '文件记录': 'route.fileRecords',
  '项目管理': 'route.projectManagement',
  '申请施工许可证': 'route.applyWorkPermit',
  
  // Vietnamese menu names (from backend)
  'Home': 'route.home',
  'Tài liệu công trình': 'route.constructionDocs',
  'Tiến độ thi công': 'route.constructionProgress',
  'Danh sách nhân viên': 'route.employeeList',
  'Quản lý vi phạm': 'route.violationManagement',
  'Tác nghiệp phát lửa': 'route.fireWork',
  'Tác nghiệp đào đất': 'route.excavationWork',
  'Tác nghiệp trên cao': 'route.heightWork',
  'Tác nghiệp điện tạm thời': 'route.temporaryElectricWork',
  'Tác nghiệp điện trực tiếp': 'route.directElectricWork',
  'Tác nghiệp cẩu móc, nâng hạ': 'route.liftingWork',
  'Tác nghiệp không gian hạn chế': 'route.confinedSpaceWork',
  'Tác nghiệp trên trần, trần lửng': 'route.ceilingWork',
  'Tác nghiệp tháo dỡ giàn giáo': 'route.scaffoldingWork',
  
  // Vietnamese menu names mapping
  'Quản lý hệ thống': 'route.system',
  'Quản lý người dùng': 'route.user',
  'Quản lý vai trò': 'route.role',
  'Quản lý menu': 'route.menu',
  'Quản lý phòng ban': 'route.dept',
  'Quản lý chức vụ': 'route.post',
  'Quản lý từ điển': 'route.dict',
  'Cài đặt tham số': 'route.config',
  'Thông báo': 'route.notice',
  'Quản lý công trình': 'route.construction',
  'Quản lý công nhân': 'route.workers',
  'Quản lý nhà thầu': 'route.contractors',
  'Quản lý khu vực': 'route.areas',
  'Các đơn tác nghiệp': 'route.workPermits',
  'Quản lý tài liệu': 'route.documents',
  'Quản lý tiến độ': 'route.progress',
  'Hồ sơ': 'route.records',
  'Công cụ hệ thống': 'route.systemTools',
  'Xây dựng biểu mẫu': 'route.formBuilding',
  'Tạo mã': 'route.codeGeneration',
  'Giao diện hệ thống': 'route.systemInterface',
  'Giám sát hệ thống': 'route.monitor',
  'Người dùng trực tuyến': 'route.online',
  'Định thời tác vụ': 'route.job',
  'Giám sát dữ liệu': 'route.druid',
  'Giám sát máy chủ': 'route.server',
  'Giám sát bộ nhớ cache': 'route.cache',
  'Quản lý nhật ký': 'route.log',
  'Nhật ký hoạt động': 'route.operlog',
  'Nhật ký đăng nhập': 'route.logininfor',
  'Trang chủ': 'route.dashboard',
  'Hồ sơ cá nhân': 'route.profile',
  'Quản lý nhà máy': 'route.factory',
  'Phân tích': 'route.analytics'
}

/**
 * Translate menu name using i18n
 * @param {string} name - Chinese menu name
 * @param {Function} t - i18n translate function
 * @returns {string} Translated name or original name if no translation found
 */
export function translateMenuName(name, t) {
  const key = menuTranslationMap[name]
  if (key) {
    return t(key)
  }
  return name
}
