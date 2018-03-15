module.exports = (store, router, demo, requester, page, vue) => ({
  blueprintPage: '/Api/1/Paper/Blueprint',

  getEntity () {
    return store.state.blueprint.entity
  },

  getPlanRoutePage () {
    if (store.state.blueprint.entity && store.state.blueprint.entity.hasLinkByRel('planRoute')) {
      return store.state.blueprint.entity.getLinkByRel('planRoute').href
    }
    return '#'
  },

  getIndexPage () {
    if (store.state.blueprint.entity && store.state.blueprint.entity.hasLinkByRel('index')) {
      return store.state.blueprint.entity.getLinkByRel('index').href
    }
    return ''
  },

  getProjectName () {
    if (this.hasProjectInfo()) {
      return store.state.blueprint.entity.properties.info.name
    }
    return ''
  },

  getProjectTitle () {
    if (this.hasProjectInfo()) {
      var title = store.state.blueprint.entity.properties.info.title
      return title !== null ? title : ''
    }
    return ''
  },

  getProjectDescription () {
    if (this.hasProjectInfo()) {
      return store.state.blueprint.entity.properties.info.description
    }
    return ''
  },

  getProjectVersion () {
    if (this.hasProjectInfo()) {
      return store.state.blueprint.entity.properties.info.version
    }
    return ''
  },

  getProjectInfo () {
    if (this.hasProjectInfo()) {
      return store.state.blueprint.entity.properties.info
    }
  },

  hasProjectInfo () {
    return store.state.blueprint.entity !== null && store.state.blueprint.entity.hasProperty('info')
  },

  hasPlanRoutePage () {
    return store.state.blueprint.entity !== null && store.state.blueprint.entity.hasLinkByRel('planRoute')
  },

  hasIndexPage () {
    return store.state.blueprint.entity != null && store.state.blueprint.entity.hasLinkByRel('index')
  },

  showNavBox () {
    if (store.state.blueprint.entity !== null && store.state.blueprint.entity.hasProperty('hasNavBox')) {
      return store.state.blueprint.entity.properties.hasNavBox === 1
    }
    return false
  },

  setBlueprint (blueprint) {
    store.commit('blueprint/setBlueprint', blueprint)
  },

  load () {
    if (store.state.blueprint.entity === null) {
      var isDemonstrationState = store.state.demonstrationState
      if (isDemonstrationState) {
        demo.loadBlueprint(this.getIndexPage)
        return
      }
      page.parse(this.blueprintPage).then(response => {
        if (response && response.ok) {
          this.setBlueprint(response.data)
        } else {
          vue.notify({ message: 'A página Blueprint (' + this.blueprintPage + ') não existe!', type: 'warning' })
        }
      })
    }
  }
})
