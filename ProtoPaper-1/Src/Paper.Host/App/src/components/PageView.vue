<template lang="pug">
  component(:is='dynamicComponent')
</template>

<script>
  // Register another component to render in this one dynamically.
  import Grid from './GridView.vue'
  import View from './View.vue'
  import Home from './HomeView.vue'
  import Actions from './ActionsView.vue'
  import { Events } from '../event-bus.js'
  export default {
    data () {
      return {
        viewShow: ''
      }
    },
    components: {
      Grid,
      View,
      Home,
      Actions
    },
    beforeRouteUpdate (to, from, next) {
      next()
      this.$store.dispatch('reloadAsync').then(() => {
        this.loadData()
      })
    },
    methods: {
      loadData () {
        var data = this.$store.state.data
        var showRightDrawer = data && (data.actions || data.links)
        Events.$emit('updateShowRightDrawer', showRightDrawer)
      }
    },
    computed: {
      dynamicComponent () {
        var data = this.$store.state.data
        var isCollection = data && data.class && data.class.indexOf('collection') > 0
        if (this.$route.query && this.$route.query.actions) {
          return Actions
        } else if (isCollection) {
          return Grid
        } else if (!isCollection) {
          return View
        } else {
          return Home
        }
      }
    },
    created () {
      this.$store.dispatch('reloadAsync').then(() => {
        this.loadData()
      })
    }
  }
</script>
