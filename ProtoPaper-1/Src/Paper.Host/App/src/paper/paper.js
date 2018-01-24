import Vue from 'vue'
import router from '../router'
import axios from 'axios'
import VueAxios from 'vue-axios'

Vue.use(VueAxios, axios)

export default {
  methods: {
    load (path) {
      var header = {
        headers: {'Accept': 'application/vnd.siren+json'}
      }
      return Vue.axios.get('/' + path, header).then(response => {
        var json = response.data
        if (json) {
          const sirenParser = require('siren-parser')
          var resource = sirenParser(json)
          return resource
        }
      }).catch(error => {
        console.log('Erro: ', error.response)
        if (error.response.status === 404) {
          router.push({name: 'notFound', params: { routerName: path }})
        } else {
          router.push({name: 'error', params: { error: error }})
        }
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
      Vue.axios.post(path, data).then(response => {
        this.loadPage(path)
      }).catch(error => {
        console.log(error.response)
      })
    },

    request (link) {
      if (link.startsWith('http')) {
        location.href = link
      } else {
        this.loadPage(link)
      }
    }
  }
}
