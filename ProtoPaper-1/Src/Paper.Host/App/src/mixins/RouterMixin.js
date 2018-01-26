export default {
  methods: {
    $_routerMixin_redirectPage (path) {
      var params = path.split('/')
      params = params.filter(function (x) {
        return (x !== (undefined || null || ''))
      })
      this.$router.push({name: 'page', params: {path: params}})
    },

    $_routerMixin_request (link) {
      if (link) {
        if (link.startsWith('http')) {
          location.href = link
        } else {
          this.$_routerMixin_redirectPage(link)
        }
      }
    },

    $_routerMixin_goToIndex () {
      var data = this.$store.state.data
      if (data && data.hasLinkByRel('self')) {
        var link = data.getLinkByRel('self')
        this.$_routerMixin_request(link.href)
      }
    },

    $_routerMixin_isIndex () {
      console.log('teste ', this.$route)
    }
  }
}
