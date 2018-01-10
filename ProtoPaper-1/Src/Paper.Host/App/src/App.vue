<template lang="pug">
  v-app
    links-bar
    action-bar
    main-toolbar
    v-content
      router-view(
        :key="$route.fullPath"
      )
    main-footer
</template>

<script>
  import ActionBar from './components/ActionBar.vue'
  import LinksBar from './components/LinksBar.vue'
  import MainFooter from './components/MainFooter.vue'
  import MainToolbar from './components/MainToolbar.vue'
  export default {
    components: {
      ActionBar,
      MainFooter,
      MainToolbar,
      LinksBar
    },
    created () {
      this.$store.dispatch('reloadAsync')
    },
    watch: {
      $route () {
        this.$store.dispatch('reloadAsync')
        this.$store.commit('selectState', false)
      }
    }
  }
</script>