const state = {
  show: false
}

const mutations = {
  updateNavigation (state, newState) {
    state.show = newState
  },

  changeNavigation (state) {
    state.show = !state.show
  },

  showNavigation (state) {
    state.show = true
  },

  hideNavigation (state) {
    state.show = false
  }
}

export default {
  state,
  mutations
}
