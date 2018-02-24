module.exports = (store, router, demo, requester, page, vue) => ({
  blueprintPage: '/Api/1/Paper/Blueprint',

  getPlanRoutePage () {
    this.loadData()
    if (store.state.blueprint.entity && store.state.blueprint.entity.hasLinkByRel('planRoute')) {
      return store.state.blueprint.entity.getLinkByRel('planRoute').href
    }
    return '#'
  },

  getIndexPage () {
    this.loadData()
    if (store.state.blueprint.entity && store.state.blueprint.entity.hasLinkByRel('index')) {
      return store.state.blueprint.entity.getLinkByRel('index').href
    }
    return '#'
  },

  hasPlanRoutePage () {
    this.loadData()
    return store.state.blueprint.entity !== null && store.state.blueprint.entity.hasLinkByRel('planRoute')
  },

  hasIndexPage () {
    this.loadData()
    return store.state.blueprint.entity != null && store.state.blueprint.entity.hasLinkByRel('index')
  },

  showNavBox () {
    this.loadData()
    if (store.state.blueprint.entity !== null && store.state.blueprint.entity.hasProperty('hasNavBox')) {
      return store.state.blueprint.entity.properties.hasNavBox === 1
    }
    return false
  },

  setBlueprint (blueprint) {
    store.commit('blueprint/setBlueprint', blueprint)
  },

  loadData () {
    if (store.state.blueprint.entity === null) {
      if (store.state.demo) {
        demo.loadBlueprint()
        return
      }
      page.parse(this.blueprintPage).then(response => {
        if (response.ok) {
          this.setBlueprint(response.data)
        } else {
          vue.notify({ message: 'A página Blueprint (' + this.blueprintPage + ') não existe!', type: 'danger' })
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
        } else if (this.hasPlanRoutePage()) {
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
