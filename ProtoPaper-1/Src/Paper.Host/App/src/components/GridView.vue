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
          <template slot="expand" slot-scope="items" v-if="$store.state.data.entities[items.index] && $store.state.data.entities[items.index].links">
            <v-list subheader>
              <v-list-tile v-for="item in $store.state.data.entities[items.index].links" :key="item.href" v-bind:href="item.href">
                <v-list-tile-content>
                  <a v-if="item.title" v-bind:href="item.href">{{ item.title }}</a>
                  <a v-else v-bind:href="item.href">{{ item.rel[0] }}</a>
                </v-list-tile-content>
              </v-list-tile>
            </v-list>
          </template>
          <template slot="links" slot-scope="items" v-if="$store.state.data.entities[items.index] && $store.state.data.entities[items.index].links">
            <v-edit-dialog
              @open="items.expanded"
              @save="props.item.iron = tmp || props.item.iron"
              large
              lazy
            >
              <div>{{ props.item.iron }}</div>
              <div slot="input" class="mt-3 title">Update Iron</div>
              <v-text-field
                slot="input"
                label="Edit"
                v-model="tmp"
                single-line
                counter
                autofocus
                :rules="[max25chars]"
              ></v-text-field>
            </v-edit-dialog>
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
        data: ''
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
