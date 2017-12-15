import Vue from 'vue'
import VueResource from 'vue-resource'
import router from '../router'

Vue.use(VueResource)

export default {
  methods: {
    load: function (path) {
      /*
      Vue.http.get('http://localhost:3000/' + path).then(response => {
        var json = response.body
        if (json) {
          this.loadPage(path, json)
        }
      }, response => {
        router.push({name: 'notFound', params: { routerName: path }})
      })
      */
      Vue.http.get(path).then(response => {
        var json = response.body
        if (json) {
          this.loadPage(path, json)
        }
      }, response => {
        router.push({name: 'notFound', params: { routerName: path }})
      })
    },

    loadPage: function (path, siren) {
      const sirenParser = require('siren-parser')
      var resource = sirenParser(siren)
      var params = path.split('/')
      params = params.filter(function (x) {
        return (x !== (undefined || null || ''))
      })
      router.push({name: 'page', params: { path: params, siren: resource }})
    },

    save: function (path, siren) {
      Vue.http.post(path, siren).then(response => {
        this.loadPage(path, siren)
      }, response => {
        console.log('error')
      })
    }
  }
}
