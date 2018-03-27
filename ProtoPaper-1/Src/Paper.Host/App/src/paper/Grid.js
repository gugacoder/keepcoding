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
          var field = this._getFieldProperties(key)
          var title = field && field.title && field.title.length > 0 ? field.title : key
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

  _getFieldProperties (fieldKey) {
    var entity = this.store.getters.entity
    if (entity && entity.properties) {
      var fields = entity.properties['_fields'] || entity.properties['fields']
      if (fields) {
        var field = fields.find((field) => field.name === fieldKey)
        return field
      }
    }
  }

}
