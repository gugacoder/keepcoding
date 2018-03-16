export default class Navigation {

  constructor (options) {
    this.store = options.store
  }

  links () {
    return this.store.getters['navigation/links']
  }

  openedRightMenu () {
    return this.store.getters['navigation/openedRightMenu']
  }

  showRightMenu () {
    return this.store.getters['navigation/showRightMenu']
  }

  changeRightMenuState () {
    this.store.commit('navigation/changeRightMenuState')
  }

  openRightMenu () {
    this.store.commit('navigation/openRightMenu')
  }

  setRightMenuVisible (visible) {
    this.store.commit('navigation/setRightMenuVisible', visible)
  }

}
