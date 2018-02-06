module.exports = (store, router, axios, error) => ({
  request (link) {
    if (link) {
      if (link.startsWith('http')) {
        if (!link.startsWith(window.location.origin)) {
          window.open(link, '_blank')
          return
        }
        link = link.replace(window.location.origin + '/page', '')
      }
      this.redirectToPage(link)
    }
  },

  redirectToPage (path) {
    var params = path.split('/')
    params = params.filter(function (x) {
      return (x !== (undefined || null || ''))
    })
    router.push({name: 'page', params: { path: params }})
  },

  httpRequest (method, href, params) {
    var getParams = method.toLowerCase() === 'get' ? params : ''
    var header = { 'Accept': 'application/json;application/vnd.siren+json;charset=UTF-8;' }
    return axios.request({
      url: href,
      method: method,
      data: params,
      params: getParams,
      headers: header
    }).then(response => {
      return {
        ok: true,
        data: response,
        message: 'Operação realizada com sucesso'
      }
    }).catch(error => {
      if (!error.response) {
        return {
          ok: false,
          data: {},
          message: 'Erro ao acessar a url'
        }
      }
      console.log('Erro: ', error.response)
      return {
        ok: false,
        data: error.response,
        message: 'Erro ao acessar a url: ' + href + '. ' + error.httpTranslate(error.response.status)
      }
    })
  },

  linkIsExternal (link) {
    return link.startsWith('http') && !link.startsWith(window.location.origin)
  },

  isOpenInAnotherPage (link) {
    return (link.type !== undefined && !link.type.match(/json/g)) || this.linkIsExternal(link.href)
  },

  target (link) {
    if (this.isOpenInAnotherPage(link)) {
      return '_blank'
    }
    return '_self'
  },

  goToIndex () {
    var data = store.state.entity
    if (data && data.hasLinkByRel('self')) {
      var link = data.getLinkByRel('self')
      this.request(link.href)
    }
  },

  containsHash (path) {
    var startsWithHash = path.startsWith(window.location.origin + '/#')
    var startsWithIndex = path.toLowerCase().startsWith(window.location.origin + '/index')
    return startsWithHash || startsWithIndex
  },

  addHashToUrl (path) {
    path = path.replace('#/', '')
    path = path.replace(window.location.origin, window.location.origin + '/#')
    return path
  },

  isRoot () {
    var isRoot = /^\/(?:|index\.html?)$/i.test(location.pathname)
    return isRoot
  }
})
