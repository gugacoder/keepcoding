<template lang="pug">
  v-toolbar(
    color="indigo"
    dark
    app
    clipped-right
    fixed
    :style="showClass"
  )
    v-btn(
      icon
      href="javascript:history.back()"
    )
      v-icon arrow_back

    v-toolbar-title(
      :style="$vuetify.breakpoint.smAndUp ? 'width: 300px; min-width: 50px' : 'min-width: 20px'" 
      class="ml-0 pl-3 "
    )
      span(
        :class="showTitle"
      ) 
        router-link(
          style="text-decoration: none; color: white"
          to="/"
        ) Sandbrowser

    v-text-field(
      v-if="$paper.blueprint.showNavBox()"
      @keyup.enter="search"
      v-model="searchParams"
      clearable
      light
      solo
      prepend-icon="search"
      placeholder="Buscar Rota"
      style="max-width: 700px; min-width: 200px"
    )

    v-spacer

    v-toolbar-side-icon(
      v-if="showLinks"
      @click.stop="openMenu"
    )
</template>

<script>
  export default {
    data: () => ({
      searchParams: '',
      demoPage: '/demo'
    }),
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      showLinks () {
        return this.$store.state.entity !== null &&
               (this.$paper.navigation.hasLinks() || this.$paper.actions.hasActions())
      },

      showClass () {
        if (this.$store.state.selection.selectionState) {
          return 'display: none'
        }
      },

      showTitle () {
        return this.$paper.blueprint.showNavBox() ? 'hidden-xs-only' : ''
      }
    },
    methods: {
      search () {
        this.$paper.requester.request(this.searchParams)
      },

      clearSearch () {
        this.searchParams = ''
      },

      openMenu () {
        this.$store.commit('showNavigation')
      }
    }
  }
</script>
