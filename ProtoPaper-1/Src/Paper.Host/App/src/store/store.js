import Vue from 'vue'
import Vuex from 'vuex'

import navigation from './modules/navigation.js'
import selection from './modules/selection.js'

Vue.use(Vuex)

const state = {
  data: {}
}

const mutations = {
  update (state, data) {
    state.data = data
  }
}

export default new Vuex.Store({
  state,
  mutations,
  modules: {
    navigation,
    selection
  }
})
