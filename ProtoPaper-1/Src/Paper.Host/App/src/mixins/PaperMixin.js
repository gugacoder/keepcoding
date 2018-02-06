export default {
  methods: {
    $_paperMixin_load () {
      var path = this.$router.currentRoute.path
      this.$_paperMixin_loadPage(path).then(data => {
        this.$store.commit('setEntity', data)
      })
    },

    async $_paperMixin_loadPage (path) {
      if (path.match(/\/page/g)) {
        if (path.match(/\/page\/demo/g)) {
          return this.$paper.demo.load(path).then(data => {
            return data
          })
        }
        path = this.$router.currentRoute.params.path
        path = Array.isArray(path) ? path.join('/') : path
        return this.$paper.page.load(path).then(data => {
          return data
        })
      }
    }
  }
}
