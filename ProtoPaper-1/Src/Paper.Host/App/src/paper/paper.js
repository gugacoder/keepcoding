const paper = {
  install (Vue, options) {
    var parser = require('./parser.js')
    var error = require('./errors.js')
    var requester = require('./requester.js')(options.store, options.router, options.axios, error, options.vm)
    var demo = require('./demo.js')(options.store, requester, parser)
    var page = require('./page.js')(options.store, options.router, requester, parser, options.vm)
    var blueprint = require('./blueprint.js')(options.store, demo, requester, page)
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
      grid: grid
    }
    Vue.prototype.$paper = paper
  }
}

export default paper
