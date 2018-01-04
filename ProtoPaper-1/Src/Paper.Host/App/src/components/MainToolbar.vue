<template lang="pug">
  v-toolbar(
    color="indigo"
    dark
    app
    clipped-right
    fixed
    :class="showClass"
  )
    v-toolbar-side-icon(
      v-if="showLeftDrawer" 
      @click.stop="setDrawerLeft"
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
      @click.stop="setDrawerRight"
    )
</template>

<script>
  import paper from '../paper/paper.js'
  import { Events } from '../event-bus.js'
  export default {
    data: () => ({
      searchParams: '',
      drawerLeft: true,
      drawerRight: true,
      showLeftDrawer: false,
      showRightDrawer: false
    }),
    props: {
      source: String
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      showLinks () {
        Events.$emit('drawerRight', this.drawerRight)
        return this.$store.state.data && (this.$store.state.data.links || this.$store.state.data.actions)
      },
      showClass () {
        var show = this.$store.state.selectedMode ? 'hidden-sm-and-up' : 'hidden-sm-and-down'
        return show
      }
    },
    methods: {
      search () {
        paper.methods.load(this.searchParams)
      },
      clearSearch () {
        this.searchParams = ''
      },
      checkKeyUp (event) {
        if (event.key === 'Enter') {
          this.search()
        }
      },
      refreshLeftDrawer (showDrawer) {
        this.showLeftDrawer = showDrawer
      },
      refreshRightDrawer (showDrawer) {
        this.showRightDrawer = showDrawer
      },
      setDrawerLeft () {
        this.drawerLeft = !this.drawerLeft
        Events.$emit('drawerLeft', this.drawerLeft)
      },
      setDrawerRight () {
        this.drawerRight = !this.drawerRight
        Events.$emit('drawerRight', this.drawerRight)
      }
    },
    created () {
      Events.$on('updateShowLeftDrawer', this.refreshLeftDrawer)
      Events.$on('updateShowRightDrawer', this.refreshRightDrawer)
    }
  }
</script>