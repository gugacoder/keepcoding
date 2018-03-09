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
  setEntity (state, data) {
    state.entity = data
  },

  setPathEntity (state, data) {
    state.pathEntity = data
  },

  setPageState (state, data) {
    state.pageState = data
  },

  setPathName (state, data) {
    state.pathName = data
  },

  setDemonstrationState (state, data) {
    state.isDemo = data
  }
}

const getters = {
  getEntity: state => {
    return state.entity ? state.entity : {}
  }
}

export default new Vuex.Store({
  state,
  mutations,
  getters,
  modules: {
    navigation,
    selection,
    blueprint
  }
})
