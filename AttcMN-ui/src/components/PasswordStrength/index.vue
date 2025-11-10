<template>
    <div class="tips">
      <span class="left5" :class="pwdMark >= 1 ? 'colorRed' : 'colorInit'"></span>
      <span class="left5" :class="pwdMark >= 25 ? 'colorOrange' : 'colorInit'"></span>
      <span class="left5" :class="pwdMark >= 50 ? 'colorOrange' : 'colorInit'"></span>
      <span class="left5" :class="pwdMark >= 60 ? 'colorGreen' : 'colorInit'"></span>
      <span class="left5" :class="pwdMark >= 70 ? 'colorGreen' : 'colorInit'"></span>
      <span class="left5" :class="pwdMark >= 80 ? 'colorSafe' : 'colorInit'"></span>
      <span class="left5" :class="pwdMark >= 90 ? 'colorSafe' : 'colorInit'"></span>
    </div>
</template>

<script>
  export default {
    props: {
      password: String
    },
    computed: {
      pwdMark() {
        let str = this.password;
        // 第一步先判断各分类长度
        let num = 0; //数字
        let lowerCase = 0; //小写字母
        let upperCase = 0; //大写字母
        let special = 0; //特殊字符
        for (let i = 0; i < str.length; i++) {
          let c = str.charCodeAt(i);
          if (c >= 48 && c <= 57) {
            //数字
            num++;
          } else if (c >= 65 && c <= 90) {
            //大写字母
            upperCase++;
          } else if (c >= 97 && c <= 122) {
            //小写字母
            lowerCase++;
          } else {
            special++;
          }
        }
        // 第二步 计算分数
        let wholeMark = 0;
        let numMark = 0;
        let caseMark = 0;
        let specialMark = 0;
        let reward = 0;
        // 整长度分数
        if (str.length <= 4) {
          wholeMark = 0;
        } else if (str.length > 4 && str.length <= 8) {
          wholeMark = 10;
        } else {
          wholeMark = 25;
        }
        // 奖励分数
        if ((lowerCase || upperCase) && num) {
          reward = 2;
          if (special) {
            reward = 5;
            if (lowerCase && upperCase) {
              reward = 10;
            }
          }
        }
        // 字母分数
        if (lowerCase || upperCase) {
          caseMark = 10;
          if (lowerCase && upperCase) {
            caseMark = 20;
          }
        }
        // 数字分数
        if (num) {
          numMark = 10;
          if (num >= 3) {
            numMark = 20;
          }
        }
        // 符号分数
        if (special) {
          specialMark = 10;
          if (special > 1) {
            specialMark = 25;
          }
        }
        return wholeMark + numMark + caseMark + specialMark + reward;
      }
    },
    methods: {
      getText(num) {
        if (num < 25) {
          return '非常弱';
        } else if (num >= 25 && num < 50) {
          return '弱';
        } else if (num >= 50 && num < 60) {
          return '一般';
        } else if (num >= 60 && num < 70) {
          return '强';
        } else if (num >= 70 && num < 80) {
          return '非常强';
        } else if (num >= 80 && num < 90) {
          return '安全';
        } else {
          return '非常安全';
        }
      }
    }
  };
</script>

<style scoped>
  .tips {
    display: flex;
    align-items: center;
    margin: 6px 0;
  }
  .tips span {
    width: 24px;
    height: 5px;
    border: 1px solid transparent;
  }
  /* .tips span:last-child {
    width: 80px;
    height: 20px;
    font-size: 12px;
    line-height: 20px;
    text-align: center;
  } */
  .left5 {
    margin-left: 5px;
  }
  .colorRed {
    background-color: red;
  }
  .colorOrange {
    background-color: orange;
  }
  .colorGreen {
    background-color: #54ec51;
  }
  .colorSafe {
    background-color: #52c41a;
  }
  .colorInit {
    background: #eeeeee;
  }
</style>
