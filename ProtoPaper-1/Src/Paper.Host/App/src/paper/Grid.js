export default class Grid {

  constructor (options) {
    this.store = options.store
  }

  get selectedItems () {
    return this.store.getters['selection/items']
  }

  get items () {
    var entities = []
    var entity = this.store.getters.entity
    if (entity && entity.hasSubEntityByClass('item')) {
      entities = entity.getSubEntitiesByClass('item')
    }
    return entities
  }

  hasActions () {
    var exist = this.items.filter(entity => entity.hasAction())
    return exist && exist.length > 0
  }

  setSelectedItems (items) {
    this.store.commit('selection/setSelectedItems', items)
  }

}
