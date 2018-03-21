export default class Navigation {

  constructor (options) {
    this.store = options.store
  }

  getLinks () {
    return this.store.getters['navigation/links']
  }

  openedRightMenu () {
    return this.store.getters['navigation/openedRightMenu']
  }

  showRightMenu () {
    return this.store.getters['navigation/showRightMenu']
  }

  openRightMenu () {
    this.store.commit('navigation/openRightMenu')
  }

  closeRightMenu () {
    this.store.commit('navigation/closeRightMenu')
  }

  setRightMenuVisible (visible) {
    this.store.commit('navigation/setRightMenuVisible', visible)
  }

  changeRightMenuState () {
    this.store.commit('navigation/changeRightMenuState')
  }

}
