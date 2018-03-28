export default class Grid {

  constructor (options) {
    this.store = options.store
  }

  get selectedItems () {
    return this.store.getters['selection/items']
  }

  get items () {
    var items = []
    this.validItems.forEach((item, index) => {
      var itensWithIndex = Object.assign(
        { _indexRowItemTable: index }, item.properties
      )
      items.push(itensWithIndex)
    })
    return items
  }

  get validItems () {
    var entity = this.store.getters.entity
    if (entity && entity.hasSubEntityByClass('item')) {
      return entity.getSubEntitiesByClass('item')
    }
    return []
  }

  get headers () {
    var headers = []
    var item = this.items[0]
    if (item) {
      var keys = Object.keys(item)
      keys.forEach((key) => {
        if (!key.startsWith('_')) {
          var header = this._getHeaderProperties(key)
          var title = header && header.title && header.title.length > 0 ? header.title : key
          headers.push({
            text: title,
            align: 'left',
            sortable: false
          })
        }
      })
    }
    return headers
  }

  hasActions () {
    var exist = this.validItems.filter(entity => entity.hasAction())
    return exist && exist.length > 0
  }

  setSelectedItems (items) {
    this.store.commit('selection/setSelectedItems', items)
  }

  _getHeaderProperties (headerKey) {
    var entity = this.store.getters.entity
    if (entity && entity.properties) {
      var headers = entity.properties['_headers'] || entity.properties['headers']
      if (headers) {
        var header = headers.find((header) => header.name === headerKey)
        return header
      }
    }
  }

}
