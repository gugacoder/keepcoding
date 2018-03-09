<template lang="pug">
  component(:is='dynamicComponent')
</template>

<script>
  import PaperGrid from './GridView.vue'
  import PaperView from './View.vue'
  export default {
    data: () => ({
      viewShow: ''
    }),
    components: {
      PaperGrid,
      PaperView
    },
    beforeRouteUpdate (to, from, next) {
      if (to.params.length > 0) {
        var path = '/' + to.params.path.join('/')
        this.$store.commit('setPathEntity', path)
      }
      this.$store.commit('selectState', false)
      next()
    },
    created () {
      var path = '/' + this.$route.params.path
      if (this.$route.params.path instanceof Array) {
        path = '/' + this.$route.params.path.join('/')
      }
      this.$store.commit('setPathEntity', path)
      this.$paper.page.load()
      this.$paper.navigation.setRightMenuVisible(true)
    },
    computed: {
      dynamicComponent () {
        var data = this.$store.state.entity
        var isCollection = data && data.class && data.class.find(value => value === 'list')
        return isCollection ? PaperGrid : PaperView
      }
    }
  }
</script>
