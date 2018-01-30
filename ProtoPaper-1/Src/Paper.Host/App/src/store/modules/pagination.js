const getters = {
  showPrevious: (state, getters, rootState) => {
    return rootState.entity.hasLinkByRel('previous')
  },

  showNext: (state, getters, rootState) => {
    return rootState.entity.hasLinkByRel('next')
  },

  showFirst: (state, getters, rootState) => {
    return rootState.entity.hasLinkByRel('first')
  },

  previousLink: (state, getters, rootState) => {
    return rootState.entity.getLinkByRel('previous').href
  },

  nextLink: (state, getters, rootState) => {
    return rootState.entity.getLinkByRel('next').href
  },

  firstLink: (state, getters, rootState) => {
    return rootState.entity.getLinkByRel('first').href
  }
}

export default {
  getters,
  namespaced: true
}
