module.exports = (store) => ({
  links () {
    var items = []
    if (store.state.entity && store.state.entity.links) {
      items = store.state.entity.links.filter(
        item => item.rel.indexOf('self') &&
                item.rel.indexOf('next') &&
                item.rel.indexOf('previous') &&
                item.rel.indexOf('first')
      )
    }
    return items
  },

  hasLinks () {
    if (store.state.entity && store.state.entity.links && this.links.length > 0) {
      return true
    }
    return false
  }
})
