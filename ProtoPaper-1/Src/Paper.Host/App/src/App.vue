<template>
  <v-app id="inspire">
    <v-navigation-drawer
      fixed
      right
      clipped
      v-model="drawerRight"
      v-if="showRightDrawer"
      app>
      <v-list subheader>
        <!--v-subheader>Links</v-subheader-->
        <v-list-tile v-for="item in actions" v-bind:href="item.href" :key="item.href">
          <v-list-tile-content>
            <v-list-tile-title v-if="item.title" v-html="item.title"></v-list-tile-title>
            <v-list-tile-title v-else v-html="item.rel[0]"></v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>
    <v-toolbar
      color="indigo"
      dark
      fixed
      app
      clipped-right>
      <v-toolbar-side-icon v-if="showLeftDrawer" @click.stop="drawerLeft = !drawerLeft"></v-toolbar-side-icon>
      <v-toolbar-title :style="$vuetify.breakpoint.smAndUp ? 'width: 200px; min-width: 200px' : 'min-width: 72px'" class="ml-0 pl-3">
        <span class="hidden-xs-only">Sandbrowser</span>
      </v-toolbar-title>
      <v-text-field
        v-on:keyup="checkKeyUp"
        v-model="searchParams"
        clearable
        light
        solo
        prepend-icon="search"
        placeholder="Buscar Rota"
        style="max-width: 600px; min-width: 128px">
      </v-text-field>
      <v-spacer></v-spacer>
      <v-toolbar-side-icon v-if="showRightDrawer" @click.stop="drawerRight = !drawerRight"></v-toolbar-side-icon>
    </v-toolbar>
    <v-navigation-drawer
      fixed
      v-if="showLeftDrawer"
      v-model="drawerLeft"
      :stateless="left"
      app>
      <v-list subheader>
        <v-subheader>Filtros</v-subheader>
        <v-list-tile avatar v-for="item in filters" v-bind:href="item.href" :key="item.href" target="_blank">
          <v-list-tile-content>
            <v-list-tile-title v-html="item.title"></v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>
    <v-content>
      <router-view></router-view>
    </v-content>
    <v-footer color="indigo" app>
      <span class="white--text">KeepCoding &copy; 2017</span>
    </v-footer>
  </v-app>
</template>

<script>
  import paper from './paper/paper.js'
  import { EventBus } from './event-bus.js'
  export default {
    data: () => ({
      searchParams: '',
      drawerLeft: false,
      drawerRight: null,
      showLeftDrawer: false,
      showRightDrawer: false,
      right: false,
      left: false,
      actions: [],
      filters: []
    }),
    props: {
      source: String
    },
    methods: {
      search: function () {
        paper.methods.load(this.searchParams)
      },
      clearSearch: function () {
        this.searchParams = ''
      },
      checkKeyUp: function (event) {
        if (event.key === 'Enter') {
          this.search()
        }
      },
      refreshLeftDrawer: function (newTodo) {
        this.showLeftDrawer = newTodo
      },
      refreshRightDrawer: function (newTodo) {
        this.showRightDrawer = newTodo
      },
      setLinks: function (actions) {
        this.actions = actions
      }
    },
    created: function () {
      EventBus.$on('updateShowLeftDrawer', this.refreshLeftDrawer)
      EventBus.$on('updateShowRightDrawer', this.refreshRightDrawer)
      EventBus.$on('links', this.setLinks)
    }
  }
</script>
