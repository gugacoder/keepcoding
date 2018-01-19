import Vue from 'vue'
import VueResource from 'vue-resource'
import router from '../router'

Vue.use(VueResource)

export default {
  methods: {
    load (path) {
      return Vue.http.get('/' + path, {headers: {'Accept': 'application/vnd.siren+json'}}).then(response => {
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

    demoFileLoader (jsonFile) {
      return import(`../../static/demo${jsonFile}.json`)
    },

    loadDemo (jsonFile) {
      return this.demoFileLoader(jsonFile).then(json => {
        const sirenParser = require('siren-parser')
        var resource = sirenParser(json)
        return resource
      })
    },

    loadPage (path) {
      var params = path.split('/')
      params = params.filter(function (x) {
        return (x !== (undefined || null || ''))
      })
      router.push({name: 'page', params: {path: params}})
    },

    save (path, data) {
      Vue.http.post(path, data).then(response => {
        this.loadPage(path, data)
      }, response => {
        console.log('error ', response)
      })
    },

    request (link) {
      if (link.startsWith('http')) {
        location.href = link
      } else {
        console.log('link ', link)
        this.loadPage(link)
      }
    }
  }
}
