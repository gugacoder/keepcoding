module.exports = (store, router, demo, requester, page, vue) => ({
  blueprintPage: '/Api/1/Paper/Blueprint',

  getPlanRoutePage () {
    this.loadData()
    return store.getters['blueprint/planRoutePage']
  },

  getIndexPage () {
    this.loadData()
    return store.getters['blueprint/indexPage']
  },

  hasPlanRoutePage () {
    this.loadData()
    return store.getters['blueprint/hasRoutePage']
  },

  hasIndexPage () {
    this.loadData()
    return store.getters['blueprint/hasIndexPage']
  },

  hasNavBox () {
    this.loadData()
    return store.getters['blueprint/hasNavBox']
  },

  setBlueprint (blueprint) {
    store.commit('blueprint/setBlueprint', blueprint)
  },

  loadData () {
    if (!store.state.blueprint.entity) {
      page.parse(this.blueprintPage).then(data => {
        if (data) {
          this.setBlueprint(data)
        }
      })
    }
  },

  load () {
    page.parse(this.blueprintPage).then(response => {
      if (response.ok) {
        this.setBlueprint(response.data)
        if (this.hasIndexPage()) {
          requester.request(this.getIndexPage())
        } else if (this.hasRoutePage()) {
          router.push({name: 'notFound', params: { routerName: this.getPlanRoutePage() }})
        }
      } else {
        if (response.data && response.data.status === 404) {
          vue.notify({ message: 'A página Index (' + this.blueprintPage + ') não existe!', type: 'danger' })
        }
        router.push({name: 'error', params: { error: response.data }})
      }
    })
  }
})
