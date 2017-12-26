<template>
  <component :is='dynamicComponent' :data="sirenData"></component>
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
        viewShow: '',
        sirenData: ''
      }
    },
    components: {
      Grid,
      View,
      Home
    },
    beforeRouteUpdate (to, from, next) {
      next()
      if (!to.params.siren) {
        this.loadPage()
        return
      }
      EventBus.$emit('reset', to.params.siren)
      this.loadData(to.params.siren)
    },
    methods: {
      loadData (siren) {
        this.setLinks(siren)
        this.setActions(siren)
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
      setActions (siren) {
        if (siren && siren.actions) {
          EventBus.$emit('updateShowLeftDrawer', true)
          EventBus.$emit('actions', siren.actions)
        }
      },
      loadPage () {
        var path = this.$route.params.path
        paper.methods.loadSiren(path).then(data => {
          this.sirenData = data
          this.loadData(data)
          EventBus.$emit('reset', data)
        })
      }
    },
    computed: {
      dynamicComponent () {
        if (this.viewShow === 'Grid') {
          return Grid
        } else if (this.viewShow === 'View') {
          return View
        } else {
          return Home
        }
      }
    },
    created () {
      if (this.siren) {
        this.sirenData = this.siren
        this.loadData(this.siren)
        return
      }
      this.loadPage()
    }
  }
</script>