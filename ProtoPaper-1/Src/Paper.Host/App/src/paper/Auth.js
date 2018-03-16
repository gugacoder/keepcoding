export default class Authentication {

  constructor (options) {
    this.store = options.store
    this.axios = options.axios
    this.router = options.router
  }

  login (data) {
    this.store.dispatch('auth/request', data).then((response) => {
      console.log('success login', response)
      this.$router.push('/')
    }).catch((error) => {
      console.log('error', error)
    })
  }

  logout () {
    this.store.dispatch('auth/login').then(() => {
      this.router.push('/login')
    })
  }

}
