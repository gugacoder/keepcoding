export default class Auth {

  constructor (options) {
    this.store = options.store
    this.router = options.router
  }

  login (data) {
    this.store.dispatch('auth/request', data).then((response) => {
      this.router.push('/page/demo')
    }).catch((error) => {
      console.log('error', error)
    })
  }

  logout () {
    this.store.dispatch('auth/logout').then(() => {
      this.router.push('/login')
    })
  }

}
