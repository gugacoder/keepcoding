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
            <tr @click.stop="dialog = true">
              <td v-for="item in items.item" :key="item.key" class="text-xs-left">
                {{ item }}
              </td>
              <v-menu offset-x left bottom>
                <v-btn
                    icon
                    slot="activator">
                  <v-icon>more_vert</v-icon>
                </v-btn>
                <v-list>
                  <v-list-tile v-for="item in $store.state.data.entities[items.index].links" :key="item.href">
                    <v-list-tile-content>
                      <a v-if="item.title" v-bind:href="item.href">{{ item.title }}</a>
                      <a v-else v-bind:href="item.href">{{ item.rel[0] }}</a>
                    </v-list-tile-content>
                  </v-list-tile>
                </v-list>
              </v-menu>
            </tr>
          </template>
        </v-data-table>
      </v-container>  
    </v-card-text>
  </v-card>
</template>

<script>
  export default {
    data () {
      return {
        showLeftDrawer: '',
        headers: [],
        items: [],
        showLinks: false,
        data: '',
        dialog: false
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
      setItems () {
        var self = this
        var data = this.$store.state.data
        if (data && data.entities) {
          data.entities.forEach(function (item) {
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
      this.load()
    }
  }
</script>
