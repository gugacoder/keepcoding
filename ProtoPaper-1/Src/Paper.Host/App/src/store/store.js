import Vue from 'vue'
import Vuex from 'vuex'

import navigation from './modules/navigation.js'
import selection from './modules/selection.js'
import blueprint from './modules/blueprint.js'

Vue.use(Vuex)

const state = {
  pathEntity: null,
  entity: null,
  demo: false
}

const mutations = {
  setEntity (state, data) {
    state.entity = data
  },

  setPathEntity (state, data) {
    console.log('setPathEntity', data)
    state.pathEntity = data
  },

  setDemoState (state, data) {
    state.demo = data
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
