<template>
  <v-app id="inspire">
    <v-toolbar color="indigo" clipped-left dark fixed app>
      <v-toolbar-title :style="$vuetify.breakpoint.smAndUp ? 'width: 300px; min-width: 250px' : 'min-width: 72px'" class="ml-0 pl-3">
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
    </v-toolbar>
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
  export default {
    data () {
      return {
        searchParams: '',
        clipped: true,
        drawer: false,
        fixed: false,
        items: [{
          icon: 'bubble_chart',
          title: 'Inspire'
        }],
        miniVariant: false,
        right: true,
        rightDrawer: false,
        title: 'Vuetify.js'
      }
    },
    methods: {
      search: function () {
        this.$http.get(this.searchParams).then(response => {
          var json = response.body
          if (json) {
            paper.methods.load(json)
          }
        }, response => {
          this.$router.push({name: 'notFound', params: { routerName: this.searchParams }})
        })
      },
      clearSearch: function () {
        this.searchParams = ''
      },
      checkKeyUp: function (event) {
        if (event.key === 'Enter') {
          this.search()
        }
      }
    }
  }
</script>
