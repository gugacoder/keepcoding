module.exports = (store, router, requester, parser, vue) => ({
  title () {
    return (store.state.entity && store.state.entity.title) ? store.state.entity.title : ''
  },

  load (path) {
    path = '/' + path
    return requester.httpRequest('get', path, {}).then(response => {
      if (response.ok) {
        var json = response.data.data
        if (json) {
          return parser.parse(json)
        }
      } else {
        vue.$notify({ message: response.message, type: 'danger' })
        if (response.data.status === 404) {
          router.push({name: 'notFound', params: { routerName: path }})
        } else {
          router.push({name: 'error', params: { error: response.data }})
        }
        return null
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
