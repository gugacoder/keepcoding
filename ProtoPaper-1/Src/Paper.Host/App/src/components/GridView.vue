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
          v-model="selected"
          :headers="headers"
          :items="items"
          item-key="item.id"
          hide-actions=true
          select-all
        )
          template(
            slot="items" slot-scope="items"
          )

            v-checkbox(
              primary
              hide-details
              v-model="items.selected"
              @click="items.selected = !items.selected"
            )
            
            td(
              v-for="(item, index) in items.item"
              v-if="!index.startsWith('_')"
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
      },
      select (item) {
        if (this.selected.indexOf(item) > 0) {
          // console.log('no')
        } else {
          // console.log('yes')
          this.selected.push(item)
        }
        // console.log(item)
        // console.log('select')
      }
    },
    created () {
      Events.$on('selectState', this.selectedMode)
    },
    computed: {
      itemKey () {
        // return this.headers && this.headers.length > 0 ? this.headers[0].text : ''
        return this.$store.state.data.entities[0].properties[0]
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
            if (!key.startsWith('_')) {
              headers.push({
                text: key,
                align: 'left',
                sortable: false
              })
            }
          })
        }
        return headers
      }
    },
    watch: {
      selected () {
        // console.log('items: ', this.items['id'])
        // console.log('properties: ', this.$store.state.data.entities)
        // console.log('itens selecionados: ', this.selected)
        this.$store.commit('selectState', this.selected.length > 0)
      }
    }
  }
</script>
