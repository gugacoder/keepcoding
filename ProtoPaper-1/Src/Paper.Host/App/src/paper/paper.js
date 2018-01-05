import Vue from 'vue'
import VueResource from 'vue-resource'
import router from '../router'
import store from '../store/store'

Vue.use(VueResource)

export default {
  methods: {
    load (path) {
      Vue.http.get(path).then(response => {
        var json = response.body
        if (json) {
          const sirenParser = require('siren-parser')
          var resource = sirenParser(json)
          store.commit('update', resource)
          this.loadPage(path)
        }
      }, response => {
        store.commit('update', null)
        router.push({name: 'notFound', params: { routerName: path }})
      })
    },

    loadSiren (path) {
      return Vue.http.get('/' + path).then(response => {
        var json = response.body
        if (json) {
          const sirenParser = require('siren-parser')
          var resource = sirenParser(json)
          return resource
        }
      }, response => {
        store.commit('update', null)
        router.push({name: 'notFound', params: { routerName: path }})
      })
    },

    loadPage (path) {
      var params = path.split('/')
      params = params.filter(function (x) {
        return (x !== (undefined || null || ''))
      })
      router.push({name: 'page', params: { path: params }})
    },

    save (path, data) {
      Vue.http.post(path, data).then(response => {
        this.loadPage(path, data)
      }, response => {
        console.log('error ', response)
      })
    }
  }
}
