import Vue from 'vue'
import Vuex from 'vuex'
import paper from '../paper/paper.js'
import router from '../router'

Vue.use(Vuex)

const state = {
  data: {},
  selectedMode: false
}

const mutations = {
  update (state, data) {
    state.data = data
  },
  selectMode (state, selectedMode) {
    state.selectedMode = selectedMode
  }
}

const actions = {
  reloadAsync ({ commit }) {
    return new Promise((resolve, reject) => {
      var path = router.currentRoute.params.path
      path = Array.isArray(path) ? path.join('/') : path
      paper.methods.loadSiren(path).then(data => {
        state.data = data
        resolve()
      })
    })
  }
}

export default new Vuex.Store({
  state,
  actions,
  mutations
})
