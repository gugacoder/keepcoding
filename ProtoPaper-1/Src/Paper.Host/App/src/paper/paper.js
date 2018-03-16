import Parser from './Parser.js'
import Errors from './Errors.js'
import Demo from './Demo.js'
import Requester from './Requester.js'
import Page from './Page.js'
import Blueprint from './Blueprint.js'
import Actions from './Actions.js'
import Pagination from './Pagination.js'
import Navigation from './Navigation.js'
import Grid from './Grid.js'
import Auth from './Auth.js'

const paper = {
  install (Vue, options) {
    var error = new Errors()
    var parser = new Parser(options)
    var demo = new Demo(options, parser)
    var requester = new Requester(options, demo)
    var page = new Page(options, requester, parser, demo)
    var blueprint = new Blueprint(options, page, demo)
    var actions = new Actions(options)
    var pagination = new Pagination(options, requester)
    var navigation = new Navigation(options)
    var grid = new Grid(options)
    var auth = new Auth(options)

    var paper = {
      blueprint: blueprint,
      requester: requester,
      demo: demo,
      error: error,
      page: page,
      actions: actions,
      pagination: pagination,
      navigation: navigation,
      grid: grid,
      auth: auth,

      isPaperPage (path) {
        var isPaperPage = path.match(/\/page/g) || path.match(/page/g)
        return isPaperPage
      },

      isDemoPage (path) {
        return demo.isDemoPage(path)
      },

      goToIndexPage () {
        var indexPage = blueprint.getIndexPage()
        requester.redirectToPage(indexPage)
      }
    }

    Vue.prototype.$paper = paper
  }
}

export default paper
