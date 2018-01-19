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
      class="hidden-xs-only"
      href="javascript:history.back()"
    )
      v-icon arrow_back

    v-toolbar-title(
      :style="$vuetify.breakpoint.smAndUp ? 'width: 300px; min-width: 50px' : 'min-width: 20px'" 
      class="ml-0 pl-3 "
    )
      span(
        class="hidden-xs-only"
      ) Sandbrowser

    v-text-field(
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
      @click.stop="$store.commit('changeNavigation')"
    )
</template>

<script>
  import paper from '../paper/paper.js'
  export default {
    data: () => ({
      searchParams: '',
      demoPage: '/demo'
    }),
    props: {
      source: String
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      showLinks () {
        return this.$store.state.data && ((this.$store.state.data.links && this.$store.state.data.links.length > 1) || this.$store.state.data.actions)
      },
      showClass () {
        if (this.$store.state.selection.selectionState) {
          return 'display: none'
        }
      }
    },
    methods: {
      search () {
        paper.methods.loadPage(this.searchParams)
      },
      clearSearch () {
        this.searchParams = ''
      }
    }
  }
</script>