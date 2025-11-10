export default function useState() {
  const states = [
    { label: '签核中', value: 1, id: 1, type: 'info' },
    { label: '已通过', value: 2, id: 2, type: 'success' },
    { label: '已驳回', value: 3, id: 3, type: 'danger' },
    { label: '已过期', value: 4, id: 4, type: 'info' }
  ];
  function getState(value) {
    let result = states.find((item) => {
      return item.value == value;
    });
    return result;
  }

  const listTypes = [
    { label: '列管', value: 0, id: 1 },
    { label: '全集团列管', value: 1, id: 2 },
    { label: '解除列管', value: 2, id: 3 },
    { label: '全集团解除列管', value: 3, id: 4 }
  ];
  function getListType(value) {
    let result = listTypes.find((item) => {
      return item.value == value;
    });
    return result;
  }

  const signs = [
    { label: '待审核', value: 1, id: 1, type: 'warning' },
    { label: '已签核', value: 2, id: 2, type: 'info' },
    { label: '通过', value: 3, id: 3, type: 'success' },
    { label: '驳回', value: 4, id: 4, type: 'danger' }
  ];
  function getSign(value) {
    let result = signs.find((item) => {
      return item.value == value;
    });
    return result;
  }

  const levels = [
    { label: '一级', value: 1, id: 1, type: 'warning', meno: '永久' },
    { label: '二级', value: 2, id: 2, type: 'warning', meno: '一年' },
    { label: '三级', value: 3, id: 3, type: 'warning', meno: '三个月' }
  ];
  function getLevel(value) {
    let result = levels.find((item) => {
      return item.value == value;
    });
    return result;
  }
  return { states, getState, signs, getSign, listTypes, getListType, levels, getLevel,  };
}
