// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import Vuetify from 'vuetify'
import axios from 'axios'
import VueAxios from 'vue-axios'
import Notify from 'vue-notifyjs'

import 'vuetify/dist/vuetify.css'

import App from './App'
import router from './router'
import store from './store/store'

Vue.use(Vuetify)
Vue.use(VueAxios, axios)
Vue.use(Notify, {
  timeout: 5000,
  horizontalAlign: 'left',
  verticalAlign: 'bottom'
})

Vue.config.productionTip = false

/* eslint-disable no-new */
new Vue({
  el: '#app',
  store,
  router,
  template: '<App/>',
  components: { App }
})
