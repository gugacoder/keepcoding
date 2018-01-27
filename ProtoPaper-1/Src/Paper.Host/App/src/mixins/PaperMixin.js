import RouterMixin from './RouterMixin.js'
export default {
  mixins: [
    RouterMixin
  ],
  methods: {
    $_paperMixin_loadPaperPage (path) {
      path = '/' + path
      return this.$_routerMixin_httpRequest('get', path, {}).then(response => {
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
      })
    },

    $_paperMixin_save (path, data) {
      this.$_routerMixin_httpRequest('POST', path, data).then(response => {
        if (response.ok) {
          this.$_routerMixin_redirectPage(path)
        }
      })
    }
  }
}
