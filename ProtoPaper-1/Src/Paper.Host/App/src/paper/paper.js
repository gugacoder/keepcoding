import Vue from 'vue'
import VueResource from 'vue-resource'
import router from '../router'

Vue.use(VueResource)

export default {
  methods: {
    load (path) {
      Vue.http.get('http://localhost:3000/' + path).then(response => {
        var json = response.body
        if (json) {
          this.loadPage(path, json)
        }
      }, response => {
        router.push({name: 'notFound', params: { routerName: path }})
      })
    },

    loadSiren (path) {
      return Vue.http.get('http://localhost:3000/' + path).then(response => {
        var json = response.body
        if (json) {
          const sirenParser = require('siren-parser')
          var resource = sirenParser(json)
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
      Vue.http.post('http://localhost:3000/' + path, data).then(response => {
        this.loadPage(path, data)
      }, response => {
        console.log('error ', response)
      })
    }
  }
}
