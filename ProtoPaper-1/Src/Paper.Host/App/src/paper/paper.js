import Vue from 'vue'
import VueResource from 'vue-resource'
import router from '../router'

Vue.use(VueResource)

export default {
  methods: {
    load (path) {
      return Vue.http.get('/' + path).then(response => {
        var json = response.body
        if (json) {
          const sirenParser = require('siren-parser')
          var resource = sirenParser(json)
          return resource
        }
      }, response => {
        router.push({name: 'notFound', params: { routerName: path }})
        return null
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
