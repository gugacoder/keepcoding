export default class Grid {

  constructor (options) {
    this.store = options.store
  }

  getSelectedItems () {
    return this.store.getters['selection/items']
  }

  getItems () {
    var entities = []
    if (this.store.state.entity && this.store.state.entity.hasSubEntityByClass('item')) {
      entities = this.store.state.entity.getSubEntitiesByClass('item')
    }
    return entities
  }

  hasActions () {
    var exist = this.getItems().filter(entity => entity.hasAction())
    return exist && exist.length > 0
  }

  setSelectedItems (items) {
    this.store.commit('selection/setSelectedItems', items)
  }

}
