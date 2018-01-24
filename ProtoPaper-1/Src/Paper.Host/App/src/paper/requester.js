import Vue from 'vue'
import axios from 'axios'
import VueAxios from 'vue-axios'

Vue.use(VueAxios, axios)

export default {
  methods: {
    async request (method, href, params) {
      method = method.toLowerCase()
      if (method === 'get' && params) {
        params = { params: params }
      }
      return Vue.axios[method](href, params).then(response => {
        return {
          ok: true,
          response: response
        }
      }).catch(error => {
        console.log('Erro: ', error.response)
        return {
          ok: false,
          response: error.response
        }
      })
    }
  }
}
