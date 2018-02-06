import Vue from 'vue'
import Vuex from 'vuex'

import navigation from './modules/navigation.js'
import selection from './modules/selection.js'
import blueprint from './modules/blueprint.js'

Vue.use(Vuex)

const state = {
  entity: {}
}

const mutations = {
  setEntity (state, data) {
    state.entity = data
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
