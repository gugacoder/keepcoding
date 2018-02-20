const state = {
  entity: null
}

const mutations = {
  setBlueprint (state, entity) {
    state.entity = entity
  }
}

export default {
  state,
  mutations,
  namespaced: true
}
