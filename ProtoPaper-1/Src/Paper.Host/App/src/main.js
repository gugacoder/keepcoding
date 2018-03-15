import Vue from 'vue'
import Vuetify from 'vuetify'
import VueAxios from 'vue-axios'
import Notify from 'vue-notifyjs'
import VueMask from 'di-vue-mask'
import axios from 'axios'
import money from 'v-money'
import VueAuth from '@websanova/vue-auth'

import 'vuetify/dist/vuetify.css'

import App from './App'
import router from './router'
import store from './store/store'
import Paper from './paper/paper.js'

Vue.router = router

Vue.use(VueMask)
Vue.use(Vuetify)
Vue.use(VueAxios, axios)
Vue.use(money, {precision: 4})
Vue.use(Notify, {
  timeout: 5000,
  horizontalAlign: 'left',
  verticalAlign: 'bottom'
})
Vue.use(VueAuth, {
  auth: require('@websanova/vue-auth/drivers/auth/bearer.js'),
  http: require('@websanova/vue-auth/drivers/http/axios.1.x.js'),
  router: require('@websanova/vue-auth/drivers/router/vue-router.2.x.js'),
  rolesVar: 'type',
  loginData: { url: 'authorize', method: 'GET', redirect: '/home', fetchUser: false }
})

Vue.config.productionTip = false
Vue.axios.defaults.baseURL = process.env.API

// Adiciona o plugin do Paper
var vm = new Vue()
Vue.use(Paper, { store, router, axios, vm })

axios.interceptors.response.use((response) => {
  if (response.status === 401 &&
      ['UnauthorizedAccess', 'InvalidToken'].indexOf(response.data.code) > -1) {
    Vue.auth.logout({
      redirect: { name: 'login' }
    })
  }
  return response
}, (error) => {
  if (error.status === 401 &&
      ['UnauthorizedAccess', 'InvalidToken'].indexOf(error.data.code) > -1) {
    Vue.auth.logout({
      redirect: { name: 'login' }
    })
  }
  return error
})

/* eslint-disable no-new */
new Vue({
  el: '#app',
  store,
  router,
  template: '<App/>',
  components: { App }
})
