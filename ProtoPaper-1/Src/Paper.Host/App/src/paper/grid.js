module.exports = (store) => ({
  items () {
    var entities = []
    if (store.state.entity && store.state.entity.hasSubEntityByClass('item')) {
      entities = store.state.entity.getSubEntitiesByClass('item')
    }
    return entities
  },

  hasActions () {
    var exist = this.items.filter(entity => entity.hasAction())
    return exist && exist.length > 0
  }
})
