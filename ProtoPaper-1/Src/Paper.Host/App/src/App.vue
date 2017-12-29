<template>
  <v-app id="inspire">
<<<<<<< HEAD
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
=======
    <links></links>
>>>>>>> actions
    <v-toolbar
      color="indigo"
      dark
      fixed
      app
      clipped-right>
      <v-toolbar-side-icon v-if="showLeftDrawer" @click.stop="setDrawerLeft"></v-toolbar-side-icon>
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
      <v-toolbar-side-icon v-if="showRightDrawer" @click.stop="setDrawerRight"></v-toolbar-side-icon>
    </v-toolbar>
<<<<<<< HEAD
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
=======
    <!--actions></actions-->
>>>>>>> actions
    <v-content>
      <router-view :key="$route.fullPath"></router-view>
    </v-content>
    <v-footer color="indigo" app>
      <span class="white--text">KeepCoding &copy; 2017</span>
    </v-footer>
  </v-app>
</template>

<script>
  import paper from './paper/paper.js'
  import Actions from './components/ActionsView.vue'
  import Links from './components/MenuLinksView.vue'
  import { EventBus } from './event-bus.js'
  export default {
    data: () => ({
      searchParams: '',
      drawerLeft: true,
      drawerRight: true,
      showLeftDrawer: false,
      showRightDrawer: false
    }),
    props: {
      source: String
    },
    components: {
      Actions,
      Links
    },
    methods: {
      search () {
        paper.methods.load(this.searchParams)
      },
      clearSearch () {
        this.searchParams = ''
      },
      checkKeyUp (event) {
        if (event.key === 'Enter') {
          this.search()
        }
      },
      refreshLeftDrawer (showDrawer) {
        this.showLeftDrawer = showDrawer
      },
      refreshRightDrawer (showDrawer) {
        this.showRightDrawer = showDrawer
      },
      setDrawerLeft () {
        this.drawerLeft = !this.drawerLeft
        EventBus.$emit('drawerLeft', this.drawerLeft)
      },
      setDrawerRight () {
        this.drawerRight = !this.drawerRight
        EventBus.$emit('drawerRight', this.drawerRight)
      }
    },
    created () {
      EventBus.$on('updateShowLeftDrawer', this.refreshLeftDrawer)
      EventBus.$on('updateShowRightDrawer', this.refreshRightDrawer)
    }
  }
</script>
