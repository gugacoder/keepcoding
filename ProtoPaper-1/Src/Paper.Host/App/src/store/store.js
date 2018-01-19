import Vue from 'vue'
import Vuex from 'vuex'
import paper from '../paper/paper.js'
import router from '../router'

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

const actions = {
  reloadAsync ({ commit }) {
    var path = router.currentRoute.path
    if (path.match(/\/page/g)) {
      if (path.match(/\/page\/demo/g)) {
        return new Promise((resolve, reject) => {
          paper.methods.loadDemo(path).then(data => {
            state.data = data
            resolve()
          })
        })
      }
      path = router.currentRoute.params.path
      return new Promise((resolve, reject) => {
        path = Array.isArray(path) ? path.join('/') : path
        paper.methods.load(path).then(data => {
          state.data = data
          resolve()
        })
      })
    }
  }
}

export default new Vuex.Store({
  state,
  actions,
  mutations,
  modules: {
    navigation,
    selection
  }
})
