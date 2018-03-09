module.exports = (router, store, parser, vue) => ({
  blueprintPage: '/page/Blueprint',

  isRoot () {
    return location.hash === '#/demo'
  },

  isDemoPage (path) {
    return path !== null && (path.match(/\/page\/demo/g) !== null || path.match(/demo/g) !== null || path.match(/\/demo/g) !== null)
  },

  loadBlueprint (indexPage) {
    this._importDemoFile(this.blueprintPage).then(json => {
      var data = parser.parse(json)
      store.commit('blueprint/setBlueprint', data)
    }).catch(error => {
      console.log('erro', error)
      vue.$notify({ message: 'Erro ao carregar o blueprint: ' + this.blueprintPage, type: 'danger' })
    })
  },

  load (jsonFile) {
    store.commit('setDemonstrationState', true)
    store.commit('setPathEntity', jsonFile)
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

  _importDemoFile (jsonFile) {
    return import(`../../static/demo${jsonFile}.json`)
  },

  _makeJsonFilePath (jsonFile) {
    if (jsonFile.startsWith(location.origin)) {
      jsonFile = jsonFile.replace(location.origin, '')
    }
    if (!jsonFile.startsWith('/page')) {
      if (!jsonFile.startsWith('/')) {
        jsonFile = '/' + jsonFile
      }
      jsonFile = '/page' + jsonFile
    }
    return jsonFile
  }
})
