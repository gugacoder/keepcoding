import Vue from 'vue'
import axios from 'axios'

const state = {
  token: localStorage.getItem('user-token') || '',
  status: '',
  endpoints: {
    login: 'http://localhost:3000/auth/login'
  }
}

const getters = {
  isAuthenticated: state => !!state.token,
  authStatus: state => state.status,
}

const mutations = {

  request (state) {
    state.status = 'loading'
  },

  success (state, token) {
    state.status = 'success'
    state.token = token
  },

  error (state) {
    state.status = 'error'
  }
}

const actions = {

  request ({ commit, dispatch }, user) {
    return new Promise((resolve, reject) => {
      commit('auth/request')
      axios({ url: this.endpoints.login, data: user, method: 'POST' }).then(resp => {
        const token = resp.data.token
        localStorage.setItem('user-token', token)
        axios.defaults.headers.common['Authorization'] = token
        commit('auth/success', resp)
        // dispatch(USER_REQUEST)
        resolve(resp)
      }).catch(err => {
        commit('auth/error', err)
        localStorage.removeItem('user-token')
        reject(err)
      })
    })
  },

  logout ({commit, dispatch}) {
    return new Promise((resolve, reject) => {
      commit('auth/logout')
      localStorage.removeItem('user-token')
      delete axios.defaults.headers.common['Authorization']
      resolve()
    })
  }

}

export default {
  state,
  getters,
  mutations,
  actions
}
