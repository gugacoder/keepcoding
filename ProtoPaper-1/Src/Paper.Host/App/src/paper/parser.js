import Vue from 'vue'
import VueResource from 'vue-resource'

Vue.use(VueResource)

export default {
  methods: {
    getActions (entities) {
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
    getDiffActions (entities) {
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
    getActionsField (entities, actionName) {
      var actions = entities
                      .filter(entity => entity.actions.map(action => action.name === actionName))
                      .map(entity => entity.actions)
      var commomFields = this.getDiffFields(entities)
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
    getDiffFields (entities) {
      var flags = []
      entities.forEach(entity => {
        if (entity.hasAction()) {
          entity.actions.forEach(action => {
            action.fields.forEach(field => {
              flags[field.name] = field
            })
          })
        }
      })
      return flags
    }
  }
}
