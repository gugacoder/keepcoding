module.exports = (store, router, requester, parser, vue, demo) => ({
  getTitle () {
    return (store.state.entity && store.state.entity.title) ? store.state.entity.title : ''
  },

  isRoot () {
    var isRoot = window.location.hash.toLowerCase() === '#/index.html' || window.location.hash === '#/'
    return isRoot
  },

  load () {
    var pathEntity = store.state.pathEntity
    if (demo.isDemoPage(pathEntity)) {
      demo.load(pathEntity)
      return
    }
    requester.httpRequest('get', pathEntity, {}).then(response => {
      if (response.ok) {
        var json = response.data.data
        if (json) {
          var data = parser.parse(json)
          store.commit('setEntity', data)
        }
      } else {
        if (response.data.status === 404) {
          router.push({name: 'notFound', params: { routerName: pathEntity }})
        } else {
          router.push({name: 'error', params: { error: response.data }})
        }
      }
    })
  },

  parse (path) {
    return requester.httpRequest('get', path, {}).then(response => {
      if (response.ok) {
        var json = response.data.data
        if (json) {
          return {
            ok: true,
            data: parser.parse(json)
          }
        }
      }
      return {
        ok: false,
        data: response.data
      }
    })
  },

  save (path, data) {
    requester.httpRequest('POST', path, data).then(response => {
      if (response.ok) {
        requester.redirectToPage(path)
      }
    })
  },

  hasTitle () {
    return store.state.entity && store.state.entity.title
  }
})
