const paper = {
  install (Vue, options) {
    var parser = require('./parser.js')(options.vm)
    var error = require('./errors.js')
    var requester = require('./requester.js')(options.store, options.router, options.axios, error, options.vm)
    var demo = require('./demo.js')(options.router, options.store, requester, parser, options.vm)
    var page = require('./page.js')(options.store, options.router, requester, parser, options.vm, demo)
    var blueprint = require('./blueprint.js')(options.store, options.router, demo, requester, page, options.vm)
    var actions = require('./actions.js')(options.store, Object)
    var pagination = require('./pagination.js')(options.store, requester)
    var navigation = require('./navigation.js')(options.store)
    var grid = require('./grid.js')(options.store)
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

      isPaperPage (path) {
        return path.match(/\/page/g)
      },

      isDemoPage (path) {
        return demo.isDemoPage(path)
      },

      goToIndex () {
        var indexPage = blueprint.getIndexPage()
        requester.request(indexPage)
      }
    }
    Vue.prototype.$paper = paper
  }
}

export default paper
