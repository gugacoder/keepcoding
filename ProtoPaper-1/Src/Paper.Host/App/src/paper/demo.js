module.exports = (store, requester, parser) => ({
  blueprintPage: '/page/Api/1/Paper/Blueprint',

  isRoot () {
    return location.hash === '#/demo'
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
    return this.importDemoFile(jsonFile).then(json => {
      return parser.parse(json)
    }).catch(() => {
      // var message = 'Erro ao carregar a página de demonstração: ' + jsonFile
      // this.$notify({ message: message, type: 'danger' })
      return
    })
  },

  importDemoFile (jsonFile) {
    return import(`../../static/demo${jsonFile}.json`)
  }
})
