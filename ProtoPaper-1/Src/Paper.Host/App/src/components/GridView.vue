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
          :item-key="itemKey"
          hide-actions=true
          v-model="selected"
          select-all
        )
          template(
            slot="items" slot-scope="items"
          )
            td
              v-checkbox(
                primarytem
                hide-details
                v-model="items.selected"
              )
              
            td(
              v-for="item in items.item" 
              :key="item" 
              class="text-xs-left"
              nowrap
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
                      router-link(
                        :to="'/page' + item.href"
                      ) {{ item.title ? item.title : item.rel[0] }}

</template>

<script>
  import { Events } from '../event-bus.js'
  export default {
    data () {
      return {
        showLeftDrawer: '',
        showLinks: false,
        data: '',
        dialog: false,
        selected: []
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    methods: {
      show () {
        this.showLinks = !this.showLinks
      },
      selectedMode (selected) {
        if (!selected) {
          this.selected = []
        }
      },
      toggleAll () {
        if (this.selected.length) this.selected = []
        else this.selected = this.items.slice()
      }
    },
    created () {
      Events.$on('selectMode', this.selectedMode)
    },
    computed: {
      itemKey () {
        return this.headers && this.headers.length > 0 ? this.headers[0].text : ''
      },
      items () {
        var items = []
        var entities = this.$store.state.data.getSubEntitiesByClass('item')
        if (entities) {
          entities.forEach((item) => {
            items.push(item.properties)
          })
        }
        return items
      },
      headers () {
        var headers = []
        var item = this.items[0]
        if (item) {
          var keys = Object.keys(item)
          keys.forEach((key) => {
            headers.push({
              text: key,
              align: 'left',
              sortable: false
            })
          })
        }
        return headers
      }
    },
    watch: {
      selected () {
        this.$store.commit('selectMode', this.selected.length > 0)
      }
    }
  }
</script>
