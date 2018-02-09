module.exports = (router, store, requester, parser, vue) => ({
  blueprintPage: '/page/Api/1/Paper/Blueprint',

  isRoot () {
    return location.hash === '#/demo'
  },

  isDemoPage (path) {
    return path.match(/\/page\/demo/g) !== null
  },

  loadDemoBlueprint () {
    this.load(this.blueprintPage).then(data => {
      if (data) {
        store.commit('blueprint/setBlueprint', data)
        var indexPage = store.getters['blueprint/indexPage']
        requester.request(indexPage)
      }
    })
  },

  load (jsonFile) {
    this.importDemoFile(jsonFile).then(json => {
      var data = parser.parse(json)
      store.commit('setEntity', data)
    }).catch(() => {
      var message = 'Erro ao carregar a página de demonstração: ' + jsonFile
      vue.$notify({ message: message, type: 'danger' })
      router.push({name: 'notFound', params: { routerName: jsonFile }})
    })
  },

  importDemoFile (jsonFile) {
    return import(`../../static/demo${jsonFile}.json`)
  }
})
