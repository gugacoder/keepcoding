import Vue from 'vue'
import Vuex from 'vuex'
import paper from '../paper/paper.js'
import router from '../router'

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
  reloadAsync ({ commit, path }) {
    return new Promise((resolve, reject) => {
      var path = router.currentRoute.params.path
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
