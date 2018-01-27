import Vue from 'vue'
import router from '../router'
import axios from 'axios'
import VueAxios from 'vue-axios'
import routerMixin from '../mixins/RouterMixin'

Vue.use(VueAxios, axios)

export default {
  methods: {
    load (path) {
      path = '/' + path
      routerMixin.$_routerMixin_httpRequest('get', path, {}).then(response => {
        if (response.ok) {
          var json = response.data
          if (json) {
            return this.sirenParse(json)
          }
        } else {
          if (response.data.status === 404) {
            router.push({name: 'notFound', params: { routerName: path }})
          } else {
            router.push({name: 'error', params: { error: response.data }})
          }
        }
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
      routerMixin.$_routerMixin_httpRequest('POST', path, data).then(response => {
        if (response.ok) {
          routerMixin.$_routerMixin_redirectPage(path)
        }
      })
    }
  }
}
