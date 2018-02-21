module.exports = (router, store, requester, parser, vue) => ({
  blueprintPage: '/page/Blueprint',

  isRoot () {
    return location.hash === '#/demo'
  },

  isDemoPage (path) {
    return path.match(/\/page\/demo/g) !== null || path.match(/\/demo/g) !== null
  },

  loadBlueprint () {
    this._importDemoFile(this.blueprintPage).then(json => {
      var data = parser.parse(json)
      store.commit('blueprint/setBlueprint', data)
    }).catch(error => {
      console.log('erro', error)
      vue.$notify({ message: 'Erro ao carregar o blueprint: ' + this.blueprintPage, type: 'danger' })
    })
  },

  load (jsonFile) {
    this._setDemoState()
    jsonFile = this._makeJsonFilePath(jsonFile)
    this._importDemoFile(jsonFile).then(json => {
      var data = parser.parse(json)
      store.commit('setEntity', data)
    }).catch(() => {
      var message = 'Erro ao carregar a página de demonstração: ' + jsonFile
      vue.$notify({ message: message, type: 'danger' })
      router.push({name: 'notFound', params: { routerName: jsonFile }})
    })
  },

  _setDemoState () {
    store.commit('setDemoState', true)
  },

  _importDemoFile (jsonFile) {
    return import(`../../static/demo${jsonFile}.json`)
  },

  _makeJsonFilePath (jsonFile) {
    if (jsonFile.startsWith(location.origin)) {
      jsonFile = jsonFile.replace(location.origin, '')
    }
    if (!jsonFile.startsWith('/page')) {
      jsonFile = '/page' + jsonFile
    }
    return jsonFile
  }
})
