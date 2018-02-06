module.exports = (store, object) => ({
  getActions (entities) {
    if (!entities) {
      return []
    }
    var commomActions = this._getDiffActions(entities)
    entities.forEach(entity => {
      if (!entity.hasAction()) {
        commomActions = []
        return
      }
      var keys = object.keys(commomActions)
      var diffs = keys.filter(diff =>
        entity.actions.filter(action => action.name === diff).length === 0
      )
      diffs.forEach(diff => {
        delete commomActions[diff]
      })
    })
    return object.values(commomActions)
  },

  getActionFields (entities, actionName) {
    var actions = entities.filter(entity => entity.actions.map(action => action.name === actionName)).map(entity => entity.actions)
    var commomFields = this._getDiffFields(entities, actionName)
    actions.forEach(action => {
      action = action[0]
      var keys = object.keys(commomFields)
      var diffs = keys.filter(diff => action.fields.filter(field => field.name === diff).length === 0)
      diffs.forEach(diff => {
        delete commomFields[diff]
      })
    })
    return object.values(commomFields)
  },

  hasSubEntitiesActions () {
    var exist = store.state.entity.entities.filter(entity => entity.hasAction())
    return exist
  },

  hasActions () {
    return store.state.entity && store.state.entity.actions
  },

  getTitle (action) {
    if (action && action.title !== null && action.title !== undefined && action.title.length > 0) {
      return action.title
    }
    if (action && action.name !== null && action.name !== undefined && action.name.length > 0) {
      return action.name
    }
    return ''
  },

  _getDiffActions (entities) {
    var flags = []
    entities.forEach(entity => {
      if (entity.hasAction()) {
        entity.actions.forEach(action => {
          flags[action.name] = action
        })
      }
    })
    return flags
  },

  _getDiffFields (entities, actionName) {
    var flags = []
    entities.forEach(entity => {
      if (entity.hasAction()) {
        var action = entity.actions.find(action => action.name === actionName)
        if (action) {
          action.fields.forEach(field => {
            flags[field.name] = field
          })
        }
      }
    })
    return flags
  }
})
