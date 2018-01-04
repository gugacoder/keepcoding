const state = {
  selectedMode: false
}

const mutations = {
  selectMode (state, selectedMode) {
    state.selectedMode = selectedMode
  }
}

export default {
  state,
  mutations
}
