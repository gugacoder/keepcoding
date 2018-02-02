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
    notifications
</template>

<script>
  import ActionBar from './components/ActionBar.vue'
  import LinksBar from './components/LinksBar.vue'
  import MainFooter from './components/MainFooter.vue'
  import MainToolbar from './components/MainToolbar.vue'
  import PaperMixin from './mixins/PaperMixin.js'
  import RequestMixin from './mixins/RequestMixin.js'
  export default {
    mixins: [
      PaperMixin,
      RequestMixin
    ],
    components: {
      ActionBar,
      MainFooter,
      MainToolbar,
      LinksBar
    },
    created () {
      var containsHash = this.$_requestMixin_containsHash(window.location.href)
      console.log('containsHash ', containsHash)
      if (!containsHash) {
        var path = this.$_requestMixin_addHashToUrl(window.location.href)
        window.location = path
        return
      }
      this.$_paperMixin_load()
    },
    watch: {
      $route () {
        this.$_paperMixin_load()
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

