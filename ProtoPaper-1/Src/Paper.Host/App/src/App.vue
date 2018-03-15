<template lang="pug">
  v-app
    links-bar
    action-bar
    main-toolbar
    v-content
      router-view(:key="$route.fullPath")
    // v-content(v-else)
    //   | Loading ...
    main-footer
    notifications
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
    beforeRouteUpdate (to, from, next) {
      next()
    },
    created () {
      this.$paper.blueprint.load()
      if (this.$paper.page.isRoot() || this.$paper.demo.isRoot()) {
        return
      }
      var containsHash = this.$paper.requester.containsHash(window.location.href)
      if (!containsHash) {
        var path = this.$paper.requester.addHashToUrl(window.location.href)
        window.location = path
        return
      }
      this.load()
    },
    computed: {
      blueprint () {
        return this.$store.state.blueprint.entity
      }
    },
    methods: {
      load () {
        var validRoute = this.$router.options.routes.find(route => route.name === this.$route.name)
        var isPaperPage = this.$paper.isPaperPage(this.$route.name)
        if (isPaperPage || !validRoute) {
          var path = this.$route.path
          this.$paper.requester.redirectToPage(path)
        }
      }
    },
    watch: {
      blueprint (newQuestion, oldQuestion) {
        if (this.$paper.page.isRoot() || this.$paper.demo.isRoot()) {
          this.$paper.goToIndexPage()
        }
      }
    }
  }
</script>

<style src="vue-notifyjs/themes/material.css"></style>
<style>
  .vue-notifyjs .alert {
    z-index: 99999;
  }
</style>

