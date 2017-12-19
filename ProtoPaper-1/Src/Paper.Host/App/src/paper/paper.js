import Vue from 'vue'
import VueResource from 'vue-resource'
import router from '../router'
import { EventBus } from '../event-bus.js'

Vue.use(VueResource)

export default {
  methods: {
    load (path) {
      Vue.http.get(path).then(response => {
        var json = response.body
        if (json) {
          this.loadPage(path, json)
        }
      }, response => {
        router.push({name: 'notFound', params: { routerName: path }})
      })
    },

    loadSiren (path) {
      Vue.http.get(path).then(response => {
        var json = response.body
        if (json) {
          const sirenParser = require('siren-parser')
          var resource = sirenParser(json)
          EventBus.$emit('reset', resource)
          return resource
        }
      }, response => {
        router.push({name: 'notFound', params: { routerName: path }})
      })
    },

    loadPage (path, data) {
      const sirenParser = require('siren-parser')
      var resource = sirenParser(data)
      var params = path.split('/')
      params = params.filter(function (x) {
        return (x !== (undefined || null || ''))
      })
      router.push({name: 'page', params: { path: params, siren: resource }})
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
