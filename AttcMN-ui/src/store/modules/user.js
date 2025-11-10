import { login, logout, getInfo } from '@/api/login';
import { getToken, setToken, removeToken } from '@/utils/auth';
import defAva from '@/assets/images/profile.jpg';

const useUserStore = defineStore('user', {
  state: () => ({
    token: getToken(),
    id: '',
    name: '',
    avatar: '',
    roles: [],
    permissions: [],
    nameNo: '',
    email: '',
    factories: [], // 用户有权限的工厂列表
    selectedArea: null // 当前选中的施工区域
  }),
  actions: {
    // 登录
    login(userInfo) {
      const username = userInfo.username.trim();
      const password = userInfo.password;
      const code = userInfo.code;
      const uuid = userInfo.uuid;
      return new Promise((resolve, reject) => {
        login(username, password, code, uuid)
          .then((res) => {
            setToken(res.token);
            this.token = res.token;
            resolve();
          })
          .catch((error) => {
            reject(error);
          });
      });
    },
    // 获取用户信息
    getInfo() {
      return new Promise((resolve, reject) => {
        getInfo()
          .then((res) => {
            const user = res.user;
            const avatar =
              user.avatar == '' || user.avatar == null ? defAva : import.meta.env.VITE_APP_BASE_API + user.avatar;

            if (res.roles && res.roles.length > 0) {
              // 验证返回的roles是否是一个非空数组
              this.roles = res.roles;
              this.permissions = res.permissions;
            } else {
              this.roles = ['ROLE_DEFAULT'];
            }
            this.id = user.userId;
            this.name = user.userName;
            this.nameNo = user.nickName;
            this.avatar = avatar;
            this.email = user.email;
            // 保存用户工厂权限 - factories from response root level
            this.factories = res.factories || user.factories || [];
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      });
    },
    // 设置选中的施工区域
    setSelectedArea(area) {
      this.selectedArea = area;
    },
    // 退出系统
    logOut() {
      return new Promise((resolve, reject) => {
        logout(this.token)
          .then(() => {
            this.token = '';
            this.roles = [];
            this.permissions = [];
            removeToken();
            resolve();
          })
          .catch((error) => {
            reject(error);
          });
      });
    },
    remUser() {
      this.token = '';
      this.roles = [];
      this.permissions = [];
      removeToken();
    },
  }
});

export default useUserStore;
