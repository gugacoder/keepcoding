module.exports = (store, requester) => ({
  previousLink () {
    return this.showPrevious ? store.state.entity.getLinkByRel('previous').href : '#'
  },

  nextLink () {
    return this.showPrevious ? store.state.entity.getLinkByRel('next').href : '#'
  },

  firstLink () {
    return this.showPrevious ? store.state.entity.getLinkByRel('first').href : '#'
  },

  showPrevious () {
    return store.state.entity && store.state.entity.hasLinkByRel('previous')
  },

  showNext () {
    return store.state.entity && store.state.entity.hasLinkByRel('next')
  },

  showFirst () {
    return store.state.entity && store.state.entity.hasLinkByRel('first')
  },

  goToFirstPage () {
    requester.redirectToPage(this.firstLink())
  },

  goToNextPage () {
    requester.redirectToPage(this.nextLink())
  },

  goToPreviousPage () {
    requester.redirectToPage(this.previousLink())
  },

  getLink (page) {
    switch (page) {
      case 'next':
        return this.nextLink
      case 'previous':
        return this.previousLink
      case 'first':
        return this.previousLink
      default:
        return '#'
    }
  }
})
