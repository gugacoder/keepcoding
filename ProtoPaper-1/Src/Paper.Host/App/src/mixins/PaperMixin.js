export default {
  methods: {
    $_paperMixin_load () {
      var path = location.href
      if (this.$paper.isDemoPage(path)) {
        this.$store.commit('setDemoState')
        path = this.$router.currentRoute.path
      }
      this.$_paperMixin_loadPage(path)
    },

    $_paperMixin_loadPage (path) {
      if (this.$paper.isDemoPage(path)) {
        this.$store.commit('setDemoState')
        this.$paper.demo.load(path)
      }
      if (path.match(/\/page/g)) {
        path = path.replace('/page', '')
      }
      if (path.match(/\/#/g)) {
        path = path.replace('/#', '')
      }
      this.$store.commit('setPathEntity', path)
      this.$paper.requester.request(path)
    }
  }
}
