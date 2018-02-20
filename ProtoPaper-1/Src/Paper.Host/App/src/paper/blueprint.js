module.exports = (store, router, demo, requester, page, vue) => ({
  blueprintPage: '/Api/1/Paper/Blueprint',

  getPlanRoutePage () {
    console.log('blueprint getPlanRoutePage')
    this.loadData()
    console.log('blueprint hasPlanRoute 1', store.state.blueprint.entity.hasLinkByRel('planRoute'))
    if (store.state.blueprint.entity && store.state.blueprint.entity.hasLinkByRel('planRoute')) {
      console.log('blueprint getPlanRoute 2', store.state.blueprint.entity.getLinkByRel('planRoute'))
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

  hasNavBox () {
    this.loadData()
    if (store.state.blueprint.entity !== null && store.state.blueprint.entity.hasProperty('hasNavBox')) {
      return store.state.blueprint.entity.properties.hasNavBox
    }
    return false
  },

  setBlueprint (blueprint) {
    store.commit('blueprint/setBlueprint', blueprint)
  },

  loadData () {
    console.log('blueprint entity', store.state.blueprint.entity)
    if (store.state.blueprint.entity === null) {
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
