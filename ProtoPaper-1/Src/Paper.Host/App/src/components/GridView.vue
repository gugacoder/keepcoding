<template lang="pug">
  v-card(
    color="grey lighten-4" 
    flat
  )
    v-card-title(
      primary-title
      v-if="$store.state.data.title"
    ) 
      div
        div(
          class="headline"
        ) {{ $store.state.data.title }}
      
    v-card-text
      v-container(fluid)
        v-data-table(
            :headers="headers"
            :items="items"
            :pagination.sync="pagination"
            :rows-per-page-items="rowsPerPageItems"
            hide-actions=true
            v-model="selected"
            rows-per-page-text="Itens por página"
            select-all
            item-key="name"
        )
          template(
            slot="items" slot-scope="items"
          )
            tr(
              @click.stop="dialog = true" 
              :active="items.selected" 
              @click="items.selected = !items.selected"
            )
              td
                v-checkbox(
                  primary
                  hide-details
                  :input-value="items.selected"
                )
                
              td(
                v-for="item in items.item" 
                :key="item" 
                class="text-xs-left"
              ) {{ item }}

              td(
                class="text-xs-center" 
                @click.stop=""
              )
                v-menu(
                  offset-x 
                  left 
                  bottom 
                  v-if="$store.state.data.entities[items.index].links"
                )
                  v-btn(
                    icon
                    slot="activator"
                  )
                    v-icon
                      | more_vert

                  v-list
                    v-list-tile(
                      v-for="item in $store.state.data.entities[items.index].links" 
                      :key="item.href"
                    )
                      v-list-tile-content
                        a(
                          v-if="item.title" 
                          :href="item.href"
                        ) {{ item.title }}
                        a(
                          v-else
                          :href="item.href"
                        ) {{ item.rel[0] }}
</template>

<script>
  import { Events } from '../event-bus.js'
  export default {
    data () {
      return {
        showLeftDrawer: '',
        headers: [],
        items: [],
        showLinks: false,
        data: '',
        dialog: false,
        selected: [],
        pagination: {rowsPerPage: 10},
        rowsPerPageItems: [5, 10, 15, { text: 'Todos', value: -1 }]
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
        var entities = this.$store.state.data.getSubEntitiesByClass('collectionItem')
        if (entities) {
          entities.forEach(function (item) {
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
      },
      toggleAll () {
        if (this.selected.length) this.selected = []
        else this.selected = this.items.slice()
      }
    },
    created () {
      this.load()
    },
    watch: {
      selected () {
        this.$store.commit('selectMode', this.selected.length > 0)
        if (this.$store.state.selectedMode) {
          Events.$emit('updateShowLeftDrawer', false)
          Events.$emit('updateShowRightDrawer', false)
          Events.$emit('drawerRight', false)
        } else {
          Events.$emit('drawerRight', this.$store.state.data && this.$store.state.data.links)
        }
      }
    }
  }
</script>
