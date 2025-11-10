<!-- 签核流程组件 -->
<template>
  <div class="approval_process">
    <div :class="getSignType(item.state).class" v-for="(item, index) in signData">
      <div class="item_line">
        <div class="type">{{ item.applicationLevel }}</div>
        <div class="line" :style="index + 1 === signData.length ? { border: 'none' } : {}"></div>
      </div>
      <div class="item_info">
        <div class="user">
          {{ item.empName }}({{ item.empNo }})<span v-if="item.stopTime">停留{{ getStopTime(item.stopTime) }}</span>
        </div>
        <div><span>联系方式：</span>{{ item.email }}</div>
        <div><span>签核结果：</span ><span :style="item.state === 4 ? {color: 'red'} : {}">{{ getSignType(item.state).label }}</span></div>
        <div v-if="item.meno"><span>签核备注：</span>{{ item.meno }}</div>
        <div v-if="item.singTime"><span>操作时间：</span>{{ item.singTime }}</div>
      </div>
    </div>
  </div>
</template>

<script setup name="ApprovalProcess">
  const { proxy } = getCurrentInstance();

  // signData= {
  //   empName // 姓名
  //   empNo // 工号
  //   email // 邮箱
  //   state // 签核状态
  //   applicationLevel // 节点名称
  //   stopTime // 停留时间 number 秒
  //   singTime // 签核时间
  //   meno     // 签核备注
  // }
  const props = defineProps({
    signData: [Array]
  });
  function getSignType(id) {
    const Types = [
      { class: 'sign_item_wait', label: '未审批' },
      { class: 'sign_item_success', label: '申请' },
      { class: 'sign_item_wait', label: '待审批' },
      { class: 'sign_item_success', label: '已同意' },
      { class: 'sign_item_success', label: '已驳回' },
      { class: 'sign_item_success', label: '测试' },
      { class: 'sign_item_success', label: '测试' },
      { class: 'sign_item_fail', label: '撤回' }
    ];
    return Types[id];
  }
  function getStopTime(time) {
    let res = '';
    const M = 60;
    const H = 60 * 60;
    const D = 60 * 60 * 24;
    let day = Math.floor(time / D);
    let hour = Math.floor((time % D) / H);
    let minute = Math.floor(((time % D) % H) / M);
    let second = Math.floor(((time % D) % H) % M);

    let list = [day ? day + '天' : 0, hour ? hour + '小时' : 0, minute ? minute + '分' : 0, second ? second + '秒' : 0];
    list.forEach((item) => {
      if (item) res = res + item;
    });
    return res;
  }
</script>
<style lang="scss" scoped>
  .approval_process {
    background: #f5f7f9;
    padding: 30px 0 30px 60px;
    .sign_item_success {
      display: flex;
      margin-bottom: 10px;
      .item_line {
        display: flex;
        flex-direction: column;
        align-items: center;
        .type {
          margin-bottom: 10px;
          width: 90px;
          text-align: center;
          font-weight: 400;
          font-size: 14px;
          color: #007bfe;
        }
        .line {
          width: 0px;
          height: 107px;
          margin-top: 5px;
          background: #ffffff;
          border: 1px dashed #007bfe;
          position: relative;
          height: 100%;
          &::after {
            content: '';
            display: block;
            width: 8px;
            height: 8px;
            background: #007bfe;
            border-radius: 50%;
            position: absolute;
            top: -4px;
            left: -4px;
          }
          &::before {
            content: '';
            display: block;
            width: 14px;
            height: 14px;
            background: #D1E6FF ;
            border-radius: 50%;
            position: absolute;
            top: -7px;
            left: -7px;
          }
        }
      }
      .item_line:last-child {
        .line {
          width: 0px;
          height: 107px;
          margin-top: 5px;
          background: #ffffff;
          border: 1px dashed #ffffff;
          position: relative;
          height: 100%;
        }
      }
      .item_info {
        margin-top: 20px;
        margin-left: -15px;
        div {
          font-weight: 400;
          font-size: 12px;
          color: #666666;
          margin: 6px 0;
        }
        .user {
          font-weight: 400;
          font-size: 14px;
          color: #222222;
          margin-bottom: 10px;
          span {
            display: inline-block;
            height: 22px;
            padding: 0 6px;
            margin-left: 10px;
            background: #e5f1ff;
            border-radius: 2px;
            line-height: 22px;
            text-align: center;
            font-weight: 400;
            font-size: 12px;
            color: #007bfe;
          }
        }
      }
    }
    .sign_item_fail {
      display: flex;
      margin-bottom: 10px;
      .item_line {
        display: flex;
        flex-direction: column;
        align-items: center;
        .type {
          margin-bottom: 10px;
          width: 90px;
          font-weight: 400;
          font-size: 14px;
          color: #e35940;
          text-align: center;
        }
        .line {
          width: 0px;
          height: 107px;
          margin-top: 5px;
          background: #ffffff;
          border: 1px dashed #e35940;
          position: relative;
          height: 100%;
          &::after {
            content: '';
            display: block;
            width: 8px;
            height: 8px;
            background: #e35940;
            border-radius: 50%;
            position: absolute;
            top: -4px;
            left: -4px;
          }
          &::before {
            content: '';
            display: block;
            width: 14px;
            height: 14px;
            background: #ffc0b5;
            border-radius: 50%;
            position: absolute;
            top: -7px;
            left: -7px;
          }
        }
      }
      .item_info {
        margin-top: 20px;
        margin-left: -15px;
        div {
          font-weight: 400;
          font-size: 12px;
          color: #666666;
          margin: 6px 0;
        }
        .user {
          font-weight: 400;
          font-size: 14px;
          color: #222222;
          margin-bottom: 10px;
          span {
            display: inline-block;
            height: 22px;
            padding: 0 6px;
            margin-left: 10px;
            background: #e5f1ff;
            border-radius: 2px;
            line-height: 22px;
            text-align: center;
            font-weight: 400;
            font-size: 12px;
            color: #007bfe;
          }
        }
      }
    }
    .sign_item_wait {
      display: flex;
      margin-bottom: 10px;
      .item_line {
        display: flex;
        flex-direction: column;
        align-items: center;
        .type {
          margin-bottom: 10px;
          width: 90px;
          text-align: center;
          font-weight: 400;
          font-size: 14px;
          color: #222222;
        }
        .line {
          width: 0px;
          height: 107px;
          margin-top: 5px;
          background: #ffffff;
          border: 1px dashed #bfbfbf;
          position: relative;
          height: 100%;
          &::after {
            content: '';
            display: block;
            width: 8px;
            height: 8px;
            background: #bfbfbf;
            border-radius: 50%;
            position: absolute;
            top: -4px;
            left: -4px;
          }
          &::before {
            content: '';
            display: block;
            width: 14px;
            height: 14px;
            background: #E7E7E7;
            border-radius: 50%;
            position: absolute;
            top: -7px;
            left: -7px;
          }
        }
      }
      .item_info {
        margin-top: 20px;
        margin-left: -15px;
        div {
          font-weight: 400;
          font-size: 12px;
          color: #666666;
          margin: 6px 0;
        }
        .user {
          font-weight: 400;
          font-size: 14px;
          color: #222222;
          margin-bottom: 10px;
          span {
            display: inline-block;
            height: 22px;
            padding: 0 6px;
            margin-left: 10px;
            background: #e5f1ff;
            border-radius: 2px;
            line-height: 22px;
            text-align: center;
            font-weight: 400;
            font-size: 12px;
            color: #007bfe;
          }
        }
      }
    }
  }
</style>
