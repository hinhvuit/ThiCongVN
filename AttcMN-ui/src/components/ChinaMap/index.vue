<!-- 
  @FileDescription:中国地图
  @Date:2024-07-26 13:53:44
  @Author:LZW
-->

<template>
  <div class="app-main">
    <div ref="chinaMap" class="map"></div>
  </div>
</template>

<script setup name="ChinaMap">
  import * as echarts from 'echarts';
  // 中国地图
  import chinaJSON from './json/china.json';
  // 自定义省份背景颜色
  import regions from './json/regions.json';

  const chinaMap = ref();
  let myChart = null;

  const props = defineProps({
    // 标注的坐标
    scatter: {
      type: Array,
      default: () => {
        return [{ name: '深圳', value: [114.056961, 22.65796] }];
      }
    }
  });
  onMounted(() => {
    drawChina();
  });

  watch(
    () => props.scatter,
    (val) => {
      option.series.data = val;
      myChart && myChart.dispose();
      drawChina();
    },
    { deep: true }
  );
  let option = {
    geo: {
      map: 'china',
      roam: false, //是否允许缩放，拖拽
      zoom: 1.2, //初始化大小
      //缩放大小限制
      scaleLimit: {
        min: 1, //最小
        max: 2 //最大
      },
      //设置中心点
      center: [115.97, 28.5],
      //省份地图添加背景
      regions: regions,
      itemStyle: {
        // 默认背景颜色
        areaColor: 'rgba(0, 0, 0, 0)',
        // 显示名称颜色
        color: '#fff',
        // 省界颜色
        borderColor: '#555',
        // 省界宽度
        borderWidth: 0.5,
        emphasis: {
          areaColor: '#015ada' //鼠标移到区域高亮颜色
        }
      },
      //高亮状态
      emphasis: {
        label: {
          //文本
          color: '#fff',
          fontSize: 12
        }
      }
    },
    // 标注配置属性
    series: {
      type: 'effectScatter',
      coordinateSystem: 'geo',
      data: props.scatter,
      showEffectOn: 'render',
      rippleEffect: {
        //涟漪特效相关配置
        brushType: 'stroke' //波纹的绘制方式，可选 'stroke' 和 'fill'
      },
      // hoverAnimation: true, //是否开启鼠标 hover 的提示动画效果
      emphasis: {
        scale: 1.5 // 高亮时散点的放大比例
      },
      label: {
        //图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等，
        formatter: '{b}',
        position: 'bottom',
        color: '#015ada',
        fontSize: 18,
        fontWeight: 'bold',
        distance: 10,
        show: true
      },
      itemStyle: {
        //图形样式，是图形在默认状态下的样式；emphasis 是图形在高亮状态下的样式，比如在鼠标悬浮或者图例联动高亮时
        color: '#015ada', //散点的颜色
        radius: 1
      },
      zlevel: 1
    }
  };
  function drawChina() {
    myChart = echarts.init(chinaMap.value);
    //注册可用的地图
    echarts.registerMap('china', chinaJSON);
    myChart.setOption(option);
  }
</script>
<style lang="scss" scoped>
  .map {
    width: 100vw;
    height: calc(100vh - 84px);
    background: url('../../assets/images/beijing.jpg') no-repeat;
    background-size: 100% 100%;
  }
</style>
