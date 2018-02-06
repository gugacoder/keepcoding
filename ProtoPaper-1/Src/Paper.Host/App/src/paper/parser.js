module.exports = {
  parse (file) {
    return this.sirenParser(file)
  },

  sirenParser (file) {
    const sirenParser = require('siren-parser')
    var resource = sirenParser(file)
    return resource
  }
}
