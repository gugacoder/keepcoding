<template>
  <v-navigation-drawer
    fixed
    right
    clipped
    v-model="drawerRight"
    app>
    <v-list subheader v-if="$store.state.data.links">
      <v-subheader>NAVEGAÇÃO</v-subheader>
      <v-list-tile v-for="item in $store.state.data.links" :key="item.href" v-bind:href="item.href">
        <v-list-tile-content>
          <v-list-tile-title v-if="item.title" v-html="item.title"></v-list-tile-title>
          <v-list-tile-title v-else v-html="item.rel[0]"></v-list-tile-title>
        </v-list-tile-content>
      </v-list-tile>
    </v-list>
    <v-divider></v-divider>
    <v-list subheader v-if="$store.state.data.actions">
      <v-subheader>AÇÕES</v-subheader>
      <v-list-tile v-for="item in $store.state.data.actions" :key="item.name" :href="'/#' + $route.path + '?actions=' + item.name">
        <v-list-tile-content>
          <v-list-tile-title v-html="item.title"></v-list-tile-title>
        </v-list-tile-content>
      </v-list-tile>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
  import { EventBus } from '../event-bus.js'
  export default {
    data: () => ({
      drawerRight: false,
      right: false
    }),
    beforeRouteUpdate (to, from, next) {
      next()
    },
    created () {
      EventBus.$on('drawerRight', this.setRightDrawer)
    },
    methods: {
      setRightDrawer (drawer) {
        this.drawerRight = drawer
      }
    }
  }
</script>
