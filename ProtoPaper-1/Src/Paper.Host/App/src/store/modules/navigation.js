const state = {
  openedRightMenu: false,
  rightMenuVisible: false
}

const getters = {
  showRightMenu (state, getters, rootState) {
    var hasActions = rootState.entity && rootState.entity.actions
    return hasActions !== null && getters.hasLinks && state.rightMenuVisible
  },

  links (state, getters, rootState) {
    var items = []
    if (rootState.entity && rootState.entity.links) {
      items = rootState.entity.links.filter(
        item => item.rel.indexOf('self') &&
                item.rel.indexOf('next') &&
                item.rel.indexOf('previous') &&
                item.rel.indexOf('first')
      )
    }
    return items
  },

  hasLinks (state, getters, rootState) {
    if (rootState.entity && rootState.entity.links) {
      return getters.links.length > 0
    }
    return false
  }
}

const mutations = {
  setRightMenuVisible (state, newState) {
    state.rightMenuVisible = newState
  },

  openRightMenu (state) {
    state.openedRightMenu = true
  },

  closeRightMenu (state) {
    state.openedRightMenu = false
  }
}

export default {
  state,
  getters,
  mutations,
  namespaced: true
}
