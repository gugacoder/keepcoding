export default class Navigation {

  constructor (options) {
    this.store = options.store
  }

  get links () {
    return this.store.getters['navigation/links']
  }

  get openedRightMenu () {
    return this.store.getters['navigation/openedRightMenu']
  }

  get showRightMenu () {
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
