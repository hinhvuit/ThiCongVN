<template>
  <div class="title">
    <span>{{ title }}</span>
    <slot name="right"></slot>
  </div>
</template>

<script setup name="MyTitle">
  import { defineProps } from 'vue';
  const props = defineProps({
    title: {
      type: String,
      default: 'title',
      required: true
    }
  });
</script>
<style lang="scss" scoped>
  .title {
    height: 40px;
    width: 100%;
    padding-left: 25px;
    font-weight: 400;
    font-size: 15px;
    color: #222222;
    line-height: 40px;
    border-bottom: 2px solid #ebebeb;
    position: relative;
    display: flex;
    justify-content: space-between;
    &::before {
      content: '';
      display: block;
      position: absolute;
      top: 13px;
      left: 8px;
      border-bottom: 6px solid #ffffff;
      border-top: 6px solid #ffffff;
      border-left: 10px solid #156cff;
    }
  }
</style>
