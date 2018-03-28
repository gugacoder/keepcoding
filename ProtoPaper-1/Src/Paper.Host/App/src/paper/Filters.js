export default class Filters {

  constructor (options, actions) {
    this.store = options.store
    this.actions = actions
  }

  get all () {
    var filters = []
    if (this.actions.hasActions()) {
      filters = this.actions.getAction('filters')
    }
    return filters
  }

  hasFilters () {
    return this.all && this.all.length > 0
  }

  openMenu () {
    this.store.commit('filters/openMenu')
  }

  closeMenu () {
    this.store.commit('filters/closeMenu')
  }

  changeMenuState () {
    this.store.commit('filters/changeMenuState')
  }

}
