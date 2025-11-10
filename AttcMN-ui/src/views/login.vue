<template>
  <div class="loginBox">
    <div style="color: aliceblue;padding: 40px 30px;text-align: right;">
      <langSelect></langSelect>
    </div>
    <div class="login">
      <div class="box">
        <div class="title-box">
          <div class="title-line"></div>
          <div class="title-text">
            <!-- <span>印度安全一体化平台</span> -->
            <div>{{$t('public.IndiaAdmin')}}</div>
          </div>
          <el-image :src="loginImage" fit="fill"></el-image>
        </div>
        <div class="login-box">
          <div class="login-type">
            <span>{{$t('public.PasswordLogin')}}</span>
            <el-image :src="selectEwm" fit="fill" :lazy="true"></el-image>
          </div>
  
          <el-form ref="loginRef" :model="loginForm" :rules="loginRules" class="login-form">
            <el-form-item prop="username">
              <span class="item-title">{{$t('public.AccountInformation')}}</span>
              <el-input v-model="loginForm.username" type="text" size="large" auto-complete="off" :placeholder="$t('public.Enteaccount')">
                <template #prefix><el-icon class="el-input__icon"><User /></el-icon></template>
              </el-input>
            </el-form-item>
            <el-form-item prop="password">
              <span class="item-title">{{$t('public.AccountPassword')}}</span>
              <el-input
                v-model="loginForm.password"
                type="password"
                size="large"
                auto-complete="off"
                :placeholder="$t('public.PasswordTips')"
                @keyup.enter="handleLogin"
                show-password
              >
                <template #prefix><el-icon class="el-input__icon"><Lock /></el-icon></template>
              </el-input>
            </el-form-item>
            <el-form-item prop="code" v-if="captchaEnabled">
              <span class="item-title">{{$t('public.verificationCode')}}</span>
              <div class="code-box">
                <el-input
                  v-model="loginForm.code"
                  size="large"
                  auto-complete="off"
                  :placeholder="$t('public.VerificationCode')"
                  style="width: 57%"
                  @keyup.enter="handleLogin"
                >
                  <template #prefix><el-icon class="el-input__icon"><Key /></el-icon></template>
                </el-input>
                <div class="login-code">
                  <img :src="codeUrl" @click="getCode" class="login-code-img" />
                </div>
              </div>
            </el-form-item>
            <el-checkbox v-model="loginForm.rememberMe" style="margin: 0px 0px 25px 0px">{{$t('public.RememberThePassword')}}</el-checkbox>
            <el-form-item style="width: 100%">
              <el-button
                class="login-btn"
                :loading="loading"
                size="large"
                type="primary"
                style="width: 100%"
                @click.prevent="handleLogin"
              >
                <span v-if="!loading">{{$t('public.login')}}</span>
                <span v-else>{{$t('public.logining')}}</span>
              </el-button>
              <div style="float: right" v-if="register">
                <router-link class="link-type" :to="'/register'">{{$t('public.RegisterNow')}}</router-link>
              </div>
            </el-form-item>
            <div class="xx-link">
              <el-button type="primary" size="default" link @click="xxLogin">相信登录 >></el-button>
            </div>
          </el-form>
        </div>
      </div>
      <!--  底部  -->
      <div class="el-login-footer">
        <span></span>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { User, Lock, Key } from '@element-plus/icons-vue'
  import langSelect from '@/components/Language/Language.vue'
  import { getCodeImg } from '@/api/login';
  import Cookies from 'js-cookie';
  import loginImage from '@/assets/images/yunxingguanlihoutai.png';
  import selectEwm from '@/assets/images/erweima.png';
  import { encrypt, decrypt } from '@/utils/jsencrypt';
  import useUserStore from '@/store/modules/user';

  const userStore = useUserStore();
  const route = useRoute();
  const router = useRouter();
  const { proxy } = getCurrentInstance();

  const loginForm = ref({
    username: '',
    password: '',
    rememberMe: false,
    code: '',
    uuid: ''
  });

  const loginRules = {
    username: [{ required: true, trigger: 'blur', message: proxy.$t('public.PleaseEnterYourAccountNumber') }],
    password: [{ required: true, trigger: 'blur', message: proxy.$t('public.PleaseEnterYourPassword') }],
    code: [{ required: true, trigger: 'change', message: proxy.$t('public.PleaseEnterTheVerificationCode') }]
  };

  const codeUrl = ref('');
  const loading = ref(false);
  // 验证码开关 - Temporarily disabled due to captchaImage API error
  const captchaEnabled = ref(false);
  // 注册开关
  const register = ref(false);
  const redirect = ref(undefined);

  watch(
    route,
    (newRoute) => {
      redirect.value = newRoute.query && newRoute.query.redirect;
    },
    { immediate: true }
  );

  function handleLogin() {
    proxy.$refs.loginRef.validate((valid) => {
      if (valid) {
        loading.value = true;
        // 勾选了需要记住密码设置在 cookie 中设置记住用户名和密码
        if (loginForm.value.rememberMe) {
          Cookies.set('username-ssp-yxht', loginForm.value.username, { expires: 30 });
          Cookies.set('password-ssp-yxht', encrypt(loginForm.value.password), { expires: 30 });
          Cookies.set('rememberMe-ssp-yxht', loginForm.value.rememberMe, { expires: 30 });
        } else {
          // 否则移除
          Cookies.remove('username-ssp-yxht');
          Cookies.remove('password-ssp-yxht');
          Cookies.remove('rememberMe-ssp-yxht');
        }
        // 调用action的登录方法
        userStore
          .login(loginForm.value)
          .then(() => {
            const query = route.query;
            const otherQueryParams = Object.keys(query).reduce((acc, cur) => {
              if (cur !== 'redirect') {
                acc[cur] = query[cur];
              }
              return acc;
            }, {});
            router.push({ path: redirect.value || '/', query: otherQueryParams });
          })
          .catch(() => {
            loading.value = false;
            // 重新获取验证码
            if (captchaEnabled.value) {
              getCode();
            }
          });
      }
    });
  }

  function getCode() {
    getCodeImg().then((res) => {
      captchaEnabled.value = res.captchaEnabled === undefined ? true : res.captchaEnabled;
      if (captchaEnabled.value) {
        codeUrl.value = 'data:image/gif;base64,' + res.img;
        loginForm.value.uuid = res.uuid;
      }
    });
  }

  function getCookie() {
    const username = Cookies.get('username-ssp-yxht');
    const password = Cookies.get('password-ssp-yxht');
    const rememberMe = Cookies.get('rememberMe-ssp-yxht');
    loginForm.value = {
      username: username === undefined ? loginForm.value.username : username,
      password: password === undefined ? loginForm.value.password : decrypt(password),
      rememberMe: rememberMe === undefined ? false : Boolean(rememberMe)
    };
  }

  // TODO: xiangXinLogin API not available, temporarily disabled
  function xxLogin() {
    // const state = route.query.redirect ? route.query.redirect : '/index';
    // xiangXinLogin(state).then((res) => {
    //   location.assign(res.data);
    // });
    console.warn('xiangXinLogin API is not available');
  }

  getCode();
  getCookie();
</script>

<style lang="scss" scoped>
  .loginBox {
    /* display: flex;
    justify-content: center;
    align-items: center; */
    height: 100%;
    background-image: url('../assets/images/login-background.jpg');
    background-size: cover;
  }
  .login {
    display: flex;
    justify-content: center;
    align-items: center;
    /* height: 100%;
    background-image: url('../assets/images/login-background.jpg');
    background-size: cover; */
  }

  .title {
    margin: 0px auto 30px auto;
    text-align: center;
    color: #707070;
  }

  .login-form {
    width: 390px;
    margin: 0 auto;
    margin-top: 30px;
    ::v-deep(.el-input__inner::placeholder) {
      font-size: 13px;
    }
    .el-input {
      height: 58px;
      background: #f8f8fa;
      border-radius: 6px;
      border: 2px solid #eeeeee;
      font-size: 16px;

      input {
        height: 58px;
        background: #f8f8fa;
        border-radius: 6px;
        border: 2px solid #eeeeee;
        font-size: 16px;
      }
    }

    .input-icon {
      height: 39px;
      width: 14px;
      margin-left: 0px;
    }

    .item-title {
      font-weight: 400;
      font-size: 16px;
      color: #999999;
      margin-left: 5px;
    }

    .login-btn {
      margin-top: 25px;
      height: 58px;
      font-size: 20px;
      color: #ffffff;
    }
    .xx-link {
      width: 100%;
      margin-top: 20px;
      display: flex;
      justify-content: center;
    }
  }

  .login-tip {
    font-size: 13px;
    text-align: center;
    color: #bfbfbf;
  }
  .code-box {
    display: flex;
  }
  .login-code {
    width: 33%;
    height: 58px;
    img {
      cursor: pointer;
      vertical-align: middle;
    }
  }

  .box {
    display: flex;

    .title-box {
      width: 547px;
      height: 722px;
      padding-top: 110px;
      background: #1890ff;
      border-radius: 0px 0px 0px 60px;
      display: flex;
      flex-direction: column;
      align-items: center;
      position: relative;

      .title-line {
        width: 42px;
        height: 16px;
        background: linear-gradient(180deg, #ffffff 0%, rgba(0, 123, 254, 0) 100%);
        position: absolute;
        top: 30px;
        left: 40px;
      }

      .title-text {
        display: flex;
        flex-direction: column;
        align-items: center;
        color: #ffffff;
        font-size: 30px;

        div {
          padding: 0 15px;
          margin-top: 12px;
          margin-bottom: 60px;
          font-size: 44px;
        }
      }
    }

    .login-box {
      width: 547px;
      height: 722px;
      padding-top: 80px;
      background: #ffffff;
      border-radius: 0px 60px 0px 0px;

      .login-type {
        width: 390px;
        margin: 0 auto;
        padding: 0 5px 10px 0;
        display: flex;
        align-items: center;
        justify-content: space-between;
        border-bottom: 1px solid #e6e6e6;

        span {
          font-weight: 600;
          font-size: 20px;
          color: #0c1d2f;
          position: relative;

          &::after {
            content: '';
            display: inline-block;
            width: 75px;
            height: 6px;
            background: linear-gradient(0deg, #007bfe 0%, rgba(0, 123, 254, 0) 100%);
            position: absolute;
            left: 2px;
            bottom: 2px;
          }
        }
      }
    }
  }

  .el-login-footer {
    height: 40px;
    line-height: 40px;
    position: fixed;
    bottom: 0;
    width: 100%;
    text-align: center;
    color: #fff;
    font-family: Arial;
    font-size: 12px;
    letter-spacing: 1px;
  }

  .login-code-img {
    height: 58px;
    padding-left: 12px;
  }
</style>
