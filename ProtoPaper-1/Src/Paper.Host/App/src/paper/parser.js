module.exports = (vue) => ({
  parse (file) {
    return this.sirenParser(file)
  },

  sirenParser (file) {
    try {
      const sirenParser = require('siren-parser')
      var resource = sirenParser(file)
      return resource
    } catch (err) {
      vue.$notify({ message: 'Falhou a tentativa de realizar o parse do arquivo: ' + file, type: 'danger' })
    }
  }
})
