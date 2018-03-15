import Vue from 'vue'

const state = {
  isAuthenticated: false
}

const getters = {
  isAuthenticated () {
    return vueAuth.isAuthenticated()
  }
}

const mutations = {
  isAuthenticated (state, payload) {
    state.isAuthenticated = payload.isAuthenticated
  }
}

const actions = {

  login (context, payload) {

    Vue.auth.login({ 
      body: {
        user: payload.user, 
        password: payload.requestOptions
      }, 
      rememberMe: true
    }).then((response) => {
      context.commit('isAuthenticated', {
        isAuthenticated: Vue.auth.isAuthenticated()
      })
    })

  }
}

export default {
  state,
  getters,
  mutations,
  actions,
  namespaced: true
}
