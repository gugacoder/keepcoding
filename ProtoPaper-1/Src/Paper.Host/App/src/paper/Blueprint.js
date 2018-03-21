export default class Blueprint {

  constructor (options, page, demo, requester) {
    this.store = options.store
    this.page = page
    this.vue = options.vm
    this.requester = requester
    this.blueprintPage = '/Blueprint'
    this.blueprint = this.store.getters['blueprint/blueprint']
    this.demo = demo
  }

  getPlanRoutePage () {
    if (this.blueprint && this.blueprint.hasLinkByRel('planRoute')) {
      return this.blueprint.getLinkByRel('planRoute').href
    }
    return '#'
  }

  getIndexPage () {
    if (this.blueprint && this.blueprint.hasLinkByRel('index')) {
      return this.blueprint.getLinkByRel('index').href
    }
    return ''
  }

  getProjectName () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info.name
    }
    return ''
  }

  getProjectTitle () {
    if (this.hasProjectInfo()) {
      var title = this.blueprint.properties.info.title
      return title !== null ? title : ''
    }
    return ''
  }

  getProjectDescription () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info.description
    }
    return ''
  }

  getProjectVersion () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info.version
    }
    return ''
  }

  getProjectInfo () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info
    }
  }

  hasProjectInfo () {
    return this.blueprint && this.blueprint.hasProperty('info')
  }

  hasPlanRoutePage () {
    return this.blueprint && this.blueprint.hasLinkByRel('planRoute')
  }

  hasIndexPage () {
    return this.blueprint && this.blueprint.hasLinkByRel('index')
  }

  showNavBox () {
    if (this.blueprint && this.blueprint.hasProperty('hasNavBox')) {
      return this.blueprint.properties.hasNavBox === 1
    }
    return false
  }

  setBlueprint (blueprint) {
    this.store.commit('blueprint/setEntity', blueprint)
    this.blueprint = this.store.getters['blueprint/blueprint']
  }

  goToIndexPage () {
    var indexPage = this.getIndexPage()
    this.requester.redirectToPage(indexPage)
  }

  load () {
    return new Promise((resolve, reject) => {
      if (this.blueprint === null) {
        this.page.parse(this.blueprintPage).then(response => {
          if (response && response.ok) {
            this.setBlueprint(response.data)
          } else {
            this.vue.notify({
              message: 'A página Blueprint (' + this.blueprintPage + ') não existe!',
              type: 'warning'
            })
          }
          resolve()
        })
      } else {
        resolve()
      }
    })
  }
}
