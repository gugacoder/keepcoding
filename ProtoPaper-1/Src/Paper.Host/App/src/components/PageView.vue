<template lang="pug">
  component(:is='dynamicComponent')
</template>

<script>
  // Register another component to render in this one dynamically.
  import Grid from './GridView.vue'
  import View from './View.vue'
  import Home from './HomeView.vue'
  import Forms from './FormsView.vue'
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
      Forms
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    methods: {
      loadData () {
        var data = this.$store.state.data
        var showLinksBar = (data.actions !== undefined) || (data.links !== undefined)
        this.$store.commit('update', showLinksBar)
      }
    },
    computed: {
      dynamicComponent () {
        var data = this.$store.state.data
        var isCollection = data && data.class && data.class.indexOf('list') > 0
        if (this.$route.query && this.$route.query.actions) {
          return Forms
        } else if (isCollection) {
          return Grid
        } else if (!isCollection) {
          return View
        }
        return Home
      }
    },
    created () {
      this.$store.dispatch('reloadAsync').then(() => {
        this.loadData()
      })
    }
  }
</script>
