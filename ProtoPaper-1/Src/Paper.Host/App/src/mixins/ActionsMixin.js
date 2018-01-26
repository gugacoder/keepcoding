export default {
  methods: {
    $_actionsMixin_getActions (entities) {
      if (!entities) {
        return []
      }
      var commomActions = this.getDiffActions(entities)
      entities.forEach(entity => {
        if (!entity.hasAction()) {
          commomActions = []
          return
        }
        var keys = Object.keys(commomActions)
        var diffs = keys.filter(diff =>
          entity.actions.filter(action => action.name === diff).length === 0
        )
        diffs.forEach(diff => {
          delete commomActions[diff]
        })
      })
      return Object.values(commomActions)
    },

    $_actionsMixin_getDiffActions (entities) {
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

    $_actionsMixin_hasActions () {
      var exist = this.$store.state.data.entities.filter(entity => entity.hasAction())
      return exist
    },

    $_actionsMixin_getActionsField (entities, actionName) {
      var actions = entities
                      .filter(entity => entity.actions.map(action => action.name === actionName))
                      .map(entity => entity.actions)
      var commomFields = this.getDiffFields(entities, actionName)
      actions.forEach(action => {
        action = action[0]
        var keys = Object.keys(commomFields)
        var diffs = keys.filter(diff => action.fields.filter(field => field.name === diff).length === 0)
        diffs.forEach(diff => {
          delete commomFields[diff]
        })
      })
      return Object.values(commomFields)
    },

    $_actionsMixin_getDiffFields (entities, actionName) {
      var flags = []
      entities.forEach(entity => {
        if (entity.hasAction()) {
          var action = entity.actions.find(action => action.name === actionName)
          action.fields.forEach(field => {
            flags[field.name] = field
          })
        }
      })
      return flags
    },

    $_actionsMixin_getActionTitle (action) {
      if (action && action.title !== null && action.title !== undefined && action.title.length > 0) {
        return action.title
      }
      if (action && action.name !== null && action.name !== undefined && action.name.length > 0) {
        return action.name
      }
      return ''
    }
  }
}
