<template>
  <v-navigation-drawer
    fixed
    right
    clipped
    v-model="drawerRight"
    app>
    <v-list subheader>
      <!--v-subheader>Links</v-subheader-->
      <v-list-tile v-for="item in links" :key="item.href" v-bind:href="item.href">
        <v-list-tile-content>
          <v-list-tile-title v-if="item.title" v-html="item.title"></v-list-tile-title>
          <v-list-tile-title v-else v-html="item.rel[0]"></v-list-tile-title>
        </v-list-tile-content>
      </v-list-tile>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
  import { EventBus } from '../event-bus.js'
  export default {
    data: () => ({
      links: [],
      drawerRight: false,
      right: false
    }),
    beforeRouteUpdate (to, from, next) {
      next()
    },
    created () {
      EventBus.$on('drawerRight', this.setRightDrawer)
      EventBus.$on('links', this.setLinks)
    },
    methods: {
      setLinks (links) {
        this.links = links
      },
      setRightDrawer (drawer) {
        this.drawerRight = drawer
      }
    }
  }
</script>
