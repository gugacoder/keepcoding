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
      var action = entities.getActionsByName(actionName)
      var fields = action.fields
      fields.forEach(field => {
        console.log('field ', field)
      })
    }
  }
}
