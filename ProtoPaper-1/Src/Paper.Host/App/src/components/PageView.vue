<template>
  <component :is='dynamicComponent' :data="siren"></component>
</template>

<script>
  // Register another component to render in this one dynamically.
  import Grid from './GridView.vue'
  import View from './View.vue'
  import Home from './HomeView.vue'
  import paper from '../paper/paper.js'
  import { EventBus } from '../event-bus.js'
  export default {
    props: ['siren'],
    data () {
      return {
        viewShow: ''
      }
    },
    components: {
      Grid,
      View,
      Home
    },
    beforeRouteUpdate (to, from, next) {
      if (!to.params.siren) {
        this.loadPage()
        this.load(this.siren)
        return
      }
      console.log('beforeRouteUpdate 2', to.params.siren.class)
      EventBus.$emit('reset', to.params.siren)
      this.load(to.params.siren)
      next()
    },
    methods: {
      load (siren) {
        this.setLinks(siren)
        if (siren) {
          var isCollection = siren.class.indexOf('collection') > 0
          if (isCollection) {
            this.viewShow = 'Grid'
          } else {
            this.viewShow = 'View'
          }
        }
      },
      setLinks (siren) {
        if (siren && siren.links) {
          EventBus.$emit('updateShowRightDrawer', true)
          EventBus.$emit('links', siren.links)
        }
      },
      async loadPage () {
        var path = this.$route.params.path
        var newSiren = await paper.methods.loadSiren(path)
        this.siren = newSiren
      }
    },
    computed: {
      dynamicComponent () {
        if (this.viewShow === 'Grid') {
          Grid.data.siren = this.siren
          return Grid
        } else if (this.viewShow === 'View') {
          return View
        } else {
          return Home
        }
      }
    },
    created: function () {
      if (this.siren) {
        this.load(this.siren)
        return
      }
    }
  }
</script>