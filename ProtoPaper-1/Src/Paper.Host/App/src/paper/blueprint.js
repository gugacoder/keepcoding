module.exports = (store, demo, requester, page) => ({
  hasNavBox: store.getters['blueprint/hasNavBox'],
  indexPage: store.getters['blueprint/indexPage'],
  planRoutePage: store.getters['blueprint/planRoutePage'],
  hasRoutePage: store.getters['blueprint/hasRoutePage'],
  hasIndexPage: store.getters['blueprint/hasIndexPage'],
  blueprintPage: '/Api/1/Paper/Blueprint',

  setBlueprint (blueprint) {
    store.commit('blueprint/setBlueprint', blueprint)
  },

  load () {
    if (demo.isDemo()) {
      demo.loadDemoBlueprint()
      return
    }
    page.load(this.blueprintPage).then(data => {
      if (data) {
        this.setBlueprint(data)
        requester.request(this.indexPage.href)
      }
    })
  }
})
