import Vue from 'vue'
import Vuex from 'vuex'

import navigation from './modules/navigation.js'
import pagination from './modules/pagination.js'
import selection from './modules/selection.js'

Vue.use(Vuex)

const state = {
  entity: {}
}

const getters = {
  links: state => {
    var items = state.entity.links.filter(
      item => item.rel.indexOf('self') &&
              item.rel.indexOf('next') &&
              item.rel.indexOf('previous') &&
              item.rel.indexOf('first')
    )
    return items
  },

  hasLinks: state => {
    return state.entity.hasLinks()
  },

  showLinks: (state, getters) => {
    if (state.entity && state.entity.links && getters.links.length > 0) {
      return true
    }
    return false
  },

  showActions: state => {
    var show = state.entity && state.entity.actions
    return show
  },

  gridItems: state => {
    var entities = []
    if (state.entity && state.entity.hasSubEntityByClass('item')) {
      entities = state.entity.getSubEntitiesByClass('item')
    }
    return entities
  }
}

const mutations = {
  update (state, data) {
    state.entity = data
  }
}

export default new Vuex.Store({
  state,
  mutations,
  getters,
  modules: {
    navigation,
    selection,
    pagination
  }
})
