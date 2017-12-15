<template>
  <v-card color="grey lighten-4" flat>
    <v-card-text>
      <v-container fluid>
        <v-data-table
            hide-actions
            v-bind:headers="headers"
            v-bind:items="items"
            item-key="name">
          <template slot="items" slot-scope="items">
            <tr @click="items.expanded = !items.expanded">
              <td v-for="item in items.item" class="text-xs-left">
                {{ item }}
              </td>
            </tr>
          </template>
          <template slot="expand" slot-scope="items" v-if="siren.entities[items.index].links">
            <v-list subheader>
              <v-list-tile v-for="item in siren.entities[items.index].links" v-bind:href="item.href" target="_blank">
                <v-list-tile-content>
                  <a v-if="item.title" v-bind:href="item.href">{{ item.title }}</a>
                  <a v-else v-bind:href="item.href">{{ item.rel[0] }}</a>
                </v-list-tile-content>
              </v-list-tile>
            </v-list>
          </template>
        </v-data-table>
      </v-container>  
    </v-card-text>
  </v-card>
</template>

<script>
  import { EventBus } from '../event-bus.js'
  export default {
    props: ['siren'],
    data () {
      return {
        showLeftDrawer: '',
        headers: [],
        items: [],
        showLinks: false
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    methods: {
      show: function () {
        this.showLinks = !this.showLinks
      },
      setActions: function () {
        var hasAction = this.siren.hasActionByClass('right')
        if (hasAction) {
          var actions = this.siren.getActionsByClass('right')
          EventBus.$emit('updateShowRightDrawer', hasAction)
          EventBus.$emit('actions', actions)
        }
      },
      setFilters: function () {
        var hasFilter = this.siren.hasActionByClass('left')
        if (hasFilter) {
          var filters = this.siren.getActionsByClass('left')
          EventBus.$emit('updateShowLeftDrawer', hasFilter)
          EventBus.$emit('filters', filters)
        }
      },
      setItems: function () {
        var self = this
        this.siren.entities.forEach(function (item) {
          self.items.push(item.properties)
        })
      },
      setHeaders: function () {
        var item = this.items[0]
        var keys = Object.keys(item)
        var self = this
        keys.forEach(function (key) {
          self.headers.push({
            text: key,
            align: 'left',
            sortable: false
          })
        })
      },
      load: function () {
        this.setActions()
        this.setFilters()
        this.setItems()
        this.setHeaders()
      }
    },
    created: function () {
      if (this.siren) {
        this.load()
        return
      }
      var path = this.$route.params.path
      this.$http.get(path).then(response => {
        var json = response.body
        if (json) {
          const sirenParser = require('siren-parser')
          this.siren = sirenParser(json)
          this.load()
        }
      }, response => {
        this.$router.push({name: 'notFound', params: { routerName: path }})
      })
    }
  }
</script>
