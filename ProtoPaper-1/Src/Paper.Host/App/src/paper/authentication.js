module.exports = (store) => ({

  login (user, password) {
    store.dispatch('authentication/login', { user, password })
  }

})
