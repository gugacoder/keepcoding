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
  export default {
    mixins: [
      PaperMixin
    ],
    components: {
      ActionBar,
      MainFooter,
      MainToolbar,
      LinksBar
    },
    created () {
      this.redirect()
    },
    watch: {
      $route () {
        this.$_paperMixin_load()
      }
    },
    methods: {
      redirect () {
        console.log('pathname ', window.location.pathname)
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

