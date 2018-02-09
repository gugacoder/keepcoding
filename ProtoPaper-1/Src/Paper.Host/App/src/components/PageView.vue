<template lang="pug">
  component(:is='dynamicComponent')
</template>

<script>
  // Register another component to render in this one dynamically.
  import PaperGrid from './GridView.vue'
  import PaperView from './View.vue'
  import PaperHome from './Home.vue'
  import PaperForm from './FormView.vue'
  export default {
    data: () => ({
      viewShow: ''
    }),
    components: {
      PaperGrid,
      PaperView,
      PaperHome,
      PaperForm
    },
    beforeRouteUpdate (to, from, next) {
      this.$paper.page.load()
      next()
    },
    created () {
      this.$paper.page.load()
    },
    methods: {
      loadData () {
        var data = this.$store.state.entity
        var showLinksBar = (data.actions !== undefined) || (data.links !== undefined)
        this.$store.commit('setEntity', showLinksBar)
      }
    },
    computed: {
      dynamicComponent () {
        var data = this.$store.state.entity
        var isCollection = data && data.class && data.class.find(value => value === 'list')
        if (this.$route.query && this.$route.query.action) {
          return PaperForm
        } else if (isCollection) {
          return PaperGrid
        } else if (!isCollection) {
          return PaperView
        }
        return PaperHome
      }
    }
  }
</script>
