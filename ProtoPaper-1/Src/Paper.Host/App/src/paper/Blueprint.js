import staticBlueprint from '../../static/Blueprint.json'
import Parser from './Parser.js'
export default class Blueprint {

  constructor (options, page, demo, requester) {
    this.store = options.store
    this.page = page
    this.router = options.router
    this.vue = options.vm
    this.requester = requester
    this.blueprintPage = '/Api/1/Blueprint'
    this.blueprint = this.store.getters['blueprint/blueprint']
    this.parser = new Parser(options)
    this.demo = demo
  }

  getPlannerPage () {
    if (this.blueprint && this.blueprint.hasLinkByRel('planner')) {
      return this.blueprint.getLinkByRel('planner').href
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

  hasPlannerPage () {
    return this.blueprint && this.blueprint.hasLinkByRel('planner')
  }

  hasIndexPage () {
    return this.blueprint && this.blueprint.hasLinkByRel('index')
  }

  showNavBox () {
    if (this.blueprint && this.blueprint.hasProperty('hasNavBox')) {
      return this.blueprint.properties.hasNavBox === 1
    }
    return true
  }

  setBlueprint (blueprint) {
    this.store.commit('blueprint/setEntity', blueprint)
    this.blueprint = this.store.getters['blueprint/blueprint']
  }

  goToIndexPage () {
    var indexPage = this.getIndexPage()
    this.page.parse(indexPage).then((response) => {
      if (response && response.ok) {
        this.requester.redirectToPage(indexPage)
      }
      // else {
      //   this.router.push('Home')
      // }
    })
  }

  load () {
    return new Promise((resolve, reject) => {
      if (this.blueprint === null) {
        this.page.parse(this.blueprintPage).then(response => {
          if (response && response.ok) {
            this.setBlueprint(response.data)
          } else {
            var blueprint = this.parser.parse(staticBlueprint)
            this.setBlueprint(blueprint)
          }
          resolve()
        })
      } else {
        resolve()
      }
    })
  }
}
