<template lang="pug">
  v-toolbar(
    color="indigo"
    dark
    app
    clipped-right
    fixed
    :style="showClass"
  )
    v-toolbar-title(
      :style="$vuetify.breakpoint.smAndUp ? 'width: 200px; min-width: 200px' : 'min-width: 72px'" 
      class="ml-0 pl-3"
    )
      span(
        class="hidden-xs-only"
      )
      | Sandbrowser
    v-text-field(
      v-on:keyup="checkKeyUp"
      v-model="searchParams"
      clearable
      light
      solo
      prepend-icon="search"
      placeholder="Buscar Rota"
      style="max-width: 600px; min-width: 128px"
    )
    v-spacer
    v-toolbar-side-icon(
      v-if="showLinks"
      @click.stop="$store.commit('changeNavigation')"
    )
</template>

<script>
  import paper from '../paper/paper.js'
  export default {
    data: () => ({
      searchParams: ''
    }),
    props: {
      source: String
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      showLinks () {
        return this.$store.state.data && (this.$store.state.data.links || this.$store.state.data.actions)
      },
      showClass () {
        if (this.$store.state.selection.selectedMode) {
          return 'display: none'
        }
      }
    },
    methods: {
      search () {
        paper.methods.loadPage(this.searchParams)
        this.searchParams = ''
      },
      clearSearch () {
        this.searchParams = ''
      },
      checkKeyUp (event) {
        if (event.key === 'Enter') {
          this.search()
        }
      }
    }
  }
</script>