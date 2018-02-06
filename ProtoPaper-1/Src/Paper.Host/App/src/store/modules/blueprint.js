const state = {
  entity: null
}

const getters = {
  hasNavBox: (state) => {
    if (state.entity && state.entity.hasProperty('hasNavBox')) {
      return state.entity.properties.hasNavBox
    }
    return false
  },

  indexPage: (state) => {
    if (state.entity && state.entity.hasLinkByRel('index')) {
      return state.entity.getLinkByRel('index').href
    }
    return '#'
  },

  planRoutePage: (state) => {
    if (state.entity && state.entity.hasLinkByRel('routePlan')) {
      return state.entity.getLinkByRel('routePlan').href
    }
    return '#'
  },

  hasRoutePage: (state) => {
    return state.entity && state.entity.hasLinkByRel('routePlan')
  },

  hasIndexPage: (state) => {
    return state.entity && state.entity.hasLinkByRel('index')
  }
}

const mutations = {
  setBlueprint (state, entity) {
    state.entity = entity
  }
}

export default {
  state,
  getters,
  mutations,
  namespaced: true
}
