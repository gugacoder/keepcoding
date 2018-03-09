module.exports = (store) => ({
  links () {
    return store.getters['navigation/links']
  },

  openedRightMenu () {
    return store.getters['navigation/openedRightMenu']
  },

  showRightMenu () {
    return store.getters['navigation/showRightMenu']
  },

  openRightMenu () {
    store.commit('navigation/openRightMenu')
  },

  setRightMenuVisible (visible) {
    store.commit('navigation/setRightMenuVisible', visible)
  }
})
