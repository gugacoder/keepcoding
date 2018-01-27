import errors from '../paper/errors.js'
export default {
  methods: {
    $_routerMixin_redirectToPage (path) {
      var params = path.split('/')
      params = params.filter(function (x) {
        return (x !== (undefined || null || ''))
      })
      this.$router.push({name: 'page', params: { path: params }})
    },

    $_routerMixin_request (link) {
      if (link) {
        if (link.startsWith('http')) {
          if (link.startsWith(window.location.origin)) {
            window.location.href = link
            return
          }
          window.open(link, '_blank')
          return
        }
        this.$_routerMixin_redirectToPage(link)
      }
    },

    async $_routerMixin_httpRequest (method, href, params) {
      var getParams = method.toLowerCase() === 'get' ? params : ''
      var header = { 'Accept': 'application/json;application/vnd.siren+json;charset=UTF-8;' }
      return this.$http.request({
        url: href,
        method: method,
        data: params,
        params: getParams,
        headers: header
      }).then(response => {
        return {
          ok: true,
          data: response
        }
      }).catch(error => {
        var message = 'Erro ao acessar a url'
        if (!error.response) {
          this.$notify({ message: message, type: 'danger' })
          return {
            ok: false,
            data: {}
          }
        }
        console.log('Erro: ', error.response)
        message = message + ': ' + href + '. ' + errors.methods.translate(error.response.status)
        this.$notify({ message: message, type: 'danger' })
        return {
          ok: false,
          data: error.response
        }
      })
    },

    $_routerMixin_linkIsExternal (link) {
      return link.startsWith('http') && !link.startsWith(window.location.origin)
    },

    $_routerMixin_isOpenInAnotherPage (link) {
      return (link.type !== undefined && !link.type.match(/json/g)) || this.$_routerMixin_linkIsExternal(link.href)
    },

    $_routerMixin_target (link) {
      if (this.$_routerMixin_isOpenInAnotherPage(link)) {
        return '_blank'
      }
      return '_self'
    },

    $_routerMixin_goToIndex () {
      var data = this.$store.state.data
      if (data && data.hasLinkByRel('self')) {
        var link = data.getLinkByRel('self')
        this.$_routerMixin_request(link.href)
      }
    }
  }
}
