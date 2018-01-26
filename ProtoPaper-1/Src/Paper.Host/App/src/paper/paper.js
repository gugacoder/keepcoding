import Vue from 'vue'
import router from '../router'
import axios from 'axios'
import VueAxios from 'vue-axios'
import routerMixin from '../mixins/RouterMixin'

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
          return this.sirenParse(json)
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

    sirenParse (file) {
      const sirenParser = require('siren-parser')
      var resource = sirenParser(file)
      return resource
    },

    demoFileLoader (jsonFile) {
      return import(`../../static/demo${jsonFile}.json`)
    },

    loadDemo (jsonFile) {
      return this.demoFileLoader(jsonFile).then(json => {
        return this.sirenParse(json)
      })
    },

    save (path, data) {
      Vue.axios.post(path, data).then(response => {
        routerMixin.$_routerMixin_redirectPage(path)
      }).catch(error => {
        console.log(error.response)
      })
    }
  }
}
