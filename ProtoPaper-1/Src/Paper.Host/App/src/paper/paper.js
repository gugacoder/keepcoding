import router from '../router'
export default {
  methods: {
    print: function () {
      console.log('paper')
    },
    load: function (siren) {
      const sirenParser = require('siren-parser')

      var resource = sirenParser(siren)
      var component = resource.class[0]

      console.log('resource ', resource)
      router.push({name: 'page', params: { id: component, siren: resource }})
    }
  }
}
