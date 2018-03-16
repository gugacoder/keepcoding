import Vue from 'vue'
import Vuex from 'vuex'

import navigation from './modules/navigation.js'
import selection from './modules/selection.js'
import blueprint from './modules/blueprint.js'

Vue.use(Vuex)

const state = {
  pathEntity: null,
  entity: null,
  demonstrationState: false
}

const mutations = {

  setEntity: (state, data) => {
    state.entity = data
  },

  setEntityPath: (state, data) => {
    state.pathEntity = data
  },

  setDemonstrationState (state, data) {
    state.isDemo = data
  }

}

export default new Vuex.Store({
  state,
  mutations,
  modules: {
    navigation,
    selection,
    blueprint
  }
})
