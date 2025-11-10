import { getAllPark } from '@/api/public';
const useParkStore = defineStore('park', {
  state: () => ({
    parkList: []
  }),
  actions: {
    getAllPark() {
      getAllPark().then((res) => {
        this.parkList = res.data.map((item) => {
          return { id: item.pid, code: item.pcode, name: item.pname };
        });
      });
    },
    getPark(id) {
      return this.parkList.find((item) => item.id === id);
    }
  }
});

export default useParkStore;
