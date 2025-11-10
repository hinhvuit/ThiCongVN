<template>
  <el-form ref="pwdRef" :model="user" :rules="rules" label-width="80px">
    <el-form-item label="旧密码" prop="oldPassword">
      <div>
        <el-input
          v-model="user.oldPassword"
          placeholder="请输入旧密码"
          type="password"
          show-password
          style="width: 215px"
        />
      </div>
    </el-form-item>
    <el-form-item label="新密码" prop="newPassword">
      <div>
        <el-input
          v-model="user.newPassword"
          placeholder="请输入新密码"
          type="password"
          show-password
          style="width: 215px"
        />
        <password-strength :password="user.newPassword" />
      </div>
    </el-form-item>
    <el-form-item label="确认密码" prop="confirmPassword">
      <div>
        <el-input
          v-model="user.confirmPassword"
          placeholder="请确认新密码"
          type="password"
          show-password
          style="width: 215px"
        />
        <password-strength :password="user.confirmPassword" />
      </div>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submit">保存</el-button>
      <el-button type="danger" @click="close">关闭</el-button>
    </el-form-item>
  </el-form>
</template>

<script setup>
  import PasswordStrength from '@/components/PasswordStrength';
  import { updateUserPwd } from '@/api/system/user';

  const { proxy } = getCurrentInstance();

  const user = reactive({
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  });

  const equalToPassword = (rule, value, callback) => {
    if (user.newPassword !== value) {
      callback(new Error('两次输入的密码不一致'));
    } else {
      callback();
    }
  };
  const rules = ref({
    oldPassword: [{ required: true, message: '旧密码不能为空', trigger: 'blur' }],
    newPassword: [
      { required: true, message: '新密码不能为空', trigger: 'blur' },
      { min: 8, max: 16, message: '长度在 8 到 16 个字符', trigger: 'blur' },
      { pattern: /[a-z]/, message: '需要包含小写字母', trigger: 'blur' },
      { pattern: /[A-Z]/, message: '需要包含大写字母', trigger: 'blur' },
      { pattern: /\d/, message: '需要包含数字', trigger: 'blur' },
      {
        pattern: /[`~!@#$%^&*()_\-+=<>?:"{}|,.\/;'\\[\]·！#￥（——）：；“”‘、，|《。》？、【】[\]]/,
        message: '需要包含特殊字符',
        trigger: 'blur'
      },
      { pattern: /^[^<>"'|\\]+$/, message: '不能包含非法字符：< > " \' \\\ |', trigger: 'blur' }
    ],
    confirmPassword: [
      { required: true, message: '确认密码不能为空', trigger: 'blur' },
      { required: true, validator: equalToPassword, trigger: 'blur' },
      { min: 8, max: 16, message: '长度在 8 到 16 个字符', trigger: 'blur' },
      { pattern: /[a-z]/, message: '需要包含小写字母', trigger: 'blur' },
      { pattern: /[A-Z]/, message: '需要包含大写字母', trigger: 'blur' },
      { pattern: /\d/, message: '需要包含数字', trigger: 'blur' },
      {
        pattern: /[`~!@#$%^&*()_\-+=<>?:"{}|,.\/;'\\[\]·！#￥（——）：；“”‘、，|《。》？、【】[\]]/,
        message: '需要包含特殊字符',
        trigger: 'blur'
      },
      { pattern: /^[^<>"'|\\]+$/, message: '不能包含非法字符：< > " \' \\\ |', trigger: 'blur' }
    ]
  });

  /** 提交按钮 */
  function submit() {
    proxy.$refs.pwdRef.validate((valid) => {
      if (valid) {
        updateUserPwd(user.oldPassword, user.newPassword).then((response) => {
          proxy.$modal.msgSuccess('修改成功');
          user.oldPassword = '';
          user.newPassword = '';
          user.confirmPassword = '';
        });
      }
    });
  }
  /** 关闭按钮 */
  function close() {
    proxy.$tab.closePage();
  }
</script>
