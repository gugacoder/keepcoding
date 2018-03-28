<template lang="pug">
  v-navigation-drawer(
    fixed
    right
    clipped
    v-model="$store.state.filters.openedFiltersMenu"
    v-if="show"
    app
  )
    v-list(
      subheader
      v-if="$paper.filters.hasFilters()"
    )
      v-subheader
        | FILTROS

      v-list-tile(
        v-for="filter in $paper.filters.all"
        :key="filter.name"
        :target="$paper.requester.target(link)"
        @click.stop="$paper.requester.redirectToPage(link.href)"
      )
        v-list-tile-content
          v-list-tile-title(
            v-if="link.title" 
            v-html="link.title"
          )
          v-list-tile-title(
            v-else 
            v-html="link.rel[0]"
          )
          
</template>

<script>
  import FormsMixin from '../mixins/FormsMixin.js'
  import Breakpoint from '../mixins/Breakpoint.js'
  export default {
    beforeRouteUpdate (to, from, next) {
      next()
    },

    mixins: [
      Breakpoint,
      FormsMixin
    ],

    methods: {
      openActionPage (action) {
        var params = this.$route.params.path
        this.$paper.requester.redirectToForm(params, action.name)
      },

      openAboutPage () {
        this.$router.push({ name: 'about' })
      }
    },

    computed: {
      show () {
        var show = !this.$paper.state.selection &&
                   this.$store.state.navigation.openedRightMenu &&
                   this.$paper.navigation.showRightMenu()
        return show
      }
    }
  }
</script>
