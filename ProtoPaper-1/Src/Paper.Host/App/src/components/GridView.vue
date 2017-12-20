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
              <v-list-tile v-for="item in siren.entities[items.index].links" :key="item.rel[0]" v-bind:href="item.href">
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
    props: ['data'],
    data () {
      return {
        showLeftDrawer: '',
        headers: [],
        items: [],
        showLinks: false,
        siren: ''
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    methods: {
      show () {
        this.showLinks = !this.showLinks
      },
      load () {
        this.setItems()
        this.setHeaders()
      },
      reset (newValue) {
        Object.assign(this.$data, this.$options.data())
        this.siren = newValue
        this.load()
      },
      setItems () {
        var self = this
        if (this.siren && this.siren.entities) {
          this.siren.entities.forEach(function (item) {
            self.items.push(item.properties)
          })
        }
      },
      setHeaders () {
        var item = this.items[0]
        if (item) {
          var keys = Object.keys(item)
          var self = this
          keys.forEach(function (key) {
            self.headers.push({
              text: key,
              align: 'left',
              sortable: false
            })
          })
        }
      }
    },
    created () {
      EventBus.$on('reset', this.reset)
      this.siren = this.data
      if (this.siren) {
        this.load()
        return
      }
    }
  }
</script>
