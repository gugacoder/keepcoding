const state = {
  selectionState: false,
  itemsSelected: []
}

const mutations = {
  selectState (state, items) {
    state.selectionState = items.length > 0
    state.itemsSelected = items
  }
}

export default {
  state,
  mutations
}
