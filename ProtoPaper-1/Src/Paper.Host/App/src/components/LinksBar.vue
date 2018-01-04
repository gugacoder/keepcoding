<template lang="pug">
  v-navigation-drawer(
    fixed
    right
    clipped
    v-model="drawerRight"
    app
    v-if="showLinks || showActions"
  )
    v-list(
      subheader v-if="showLinks"
    )
      v-subheader
        | NAVEGAÇÃO
      v-list-tile(
        v-for="item in $store.state.data.links" 
        :key="item.href" 
        :href="item.href"
      )
        v-list-tile-content
          v-list-tile-title(
            v-if="item.title" 
            v-html="item.title"
          )
          v-list-tile-title(
            v-else v-html="item.rel[0]"
          )

    v-divider
    
    v-list(
      subheader 
      v-if="showActions"
    )
      v-subheader
        | AÇÕES
      v-list-tile(
        v-for="item in $store.state.data.actions" 
        :key="item.name" 
        :href="'/#' + $route.path + '?actions=' + item.name"
      )
        v-list-tile-content
          v-list-tile-title(
            v-html="item.title"
          )
</template>

<script>
  import { Events } from '../event-bus.js'
  export default {
    data: () => ({
      drawerRight: false,
      right: false
    }),
    beforeRouteUpdate (to, from, next) {
      next()
    },
    created () {
      Events.$on('drawerRight', this.setRightDrawer)
    },
    computed: {
      showLinks () {
        var show = this.$store.state.data && this.$store.state.data.links
        return show
      },
      showActions () {
        var show = this.$store.state.data && this.$store.state.data.actions
        return show
      }
    },
    methods: {
      setRightDrawer (drawer) {
        this.drawerRight = drawer
      }
    }
  }
</script>
