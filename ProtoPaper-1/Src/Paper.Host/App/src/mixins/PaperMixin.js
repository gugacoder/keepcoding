import RequestMixin from './RequestMixin.js'
export default {
  mixins: [
    RequestMixin
  ],
  methods: {
    $_paperMixin_loadPaperPage (path) {
      path = '/' + path
      return this.$_requestMixin_httpRequest('get', path, {}).then(response => {
        if (response.ok) {
          var json = response.data.data
          if (json) {
            return this.$_paperMixin_sirenParse(json)
          }
        } else {
          if (response.data.status === 404) {
            this.$router.push({name: 'notFound', params: { routerName: path }})
          } else {
            this.$router.push({name: 'error', params: { error: response.data }})
          }
          return null
        }
      })
    },

    $_paperMixin_load () {
      var path = this.$router.currentRoute.path
      if (path.match(/\/page/g)) {
        if (path.match(/\/page\/demo/g)) {
          this.$_paperMixin_loadDemo(path).then(data => {
            this.$store.commit('update', data)
          })
        } else {
          path = this.$router.currentRoute.params.path
          path = Array.isArray(path) ? path.join('/') : path
          this.$_paperMixin_loadPaperPage(path).then(data => {
            this.$store.commit('update', data)
          })
        }
      }
    },

    $_paperMixin_sirenParse (file) {
      const sirenParser = require('siren-parser')
      var resource = sirenParser(file)
      return resource
    },

    $_paperMixin_demoFileLoader (jsonFile) {
      return import(`../../static/demo${jsonFile}.json`)
    },

    $_paperMixin_loadDemo (jsonFile) {
      return this.$_paperMixin_demoFileLoader(jsonFile).then(json => {
        return this.$_paperMixin_sirenParse(json)
      }).catch(() => {
        var message = 'Erro ao carregar a página de demonstração: ' + jsonFile
        this.$notify({ message: message, type: 'danger' })
        return
      })
    },

    $_paperMixin_save (path, data) {
      this.$_requestMixin_httpRequest('POST', path, data).then(response => {
        if (response.ok) {
          this.$_requestMixin_redirectPage(path)
        }
      })
    }
  }
}
