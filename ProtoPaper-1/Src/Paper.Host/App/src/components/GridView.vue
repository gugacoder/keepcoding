<template lang="pug">
  v-card
    v-container(
      fluid
      grid-list-md
    )
      div(v-if="$paper.page.hasTitle()")
        div(class="headline") 
          | {{ $paper.page.getTitle() }}

      grid-view-pagination

      v-data-table(
        v-model="selected"
        :headers="headers"
        :items="items"
        item-key="_indexRowItemTable"
        hide-actions=true
        :select-all="$paper.grid.hasActions()"
        no-data-text="Não existem dados para exibir"
      )
        template(
          slot="items" 
          slot-scope="items"
        )

          td(v-if="$paper.grid.hasActions()")
            v-checkbox(
              primary
              hide-details
              v-model="items.selected"
              @click.stop="items.selected = !items.selected"
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
              v-if="$store.state.entity.entities[items.index].links"
            )
              v-btn(
                icon
                slot="activator"
              )
                v-icon
                  | more_vert

              v-list
                v-list-tile(
                  v-for="item in $store.state.entity.entities[items.index].links" 
                  :key="item.href"
                )
                  v-list-tile-content
                    a(
                      @click.stop="$paper.requester.request(item.href)"
                    ) {{ item.title ? item.title : item.rel[0] }}

</template>

<script>
  import { Events } from '../event-bus.js'
  import GridViewPagination from './GridViewPagination.vue'
  export default {
    components: {
      GridViewPagination
    },
    data: () => ({
      showLeftDrawer: '',
      showLinks: false,
      selected: [],
      bottom: false
    }),
    created () {
      Events.$on('selectState', this.selectedMode)
      window.addEventListener('scroll', () => {
        this.bottom = true
      })
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
    computed: {
      items () {
        var items = []
        if (this.$paper.grid.getItems()) {
          this.$paper.grid.getItems().forEach((item, index) => {
            var itensWithIndex = Object.assign(
              { _indexRowItemTable: index }, item.properties
            )
            items.push(itensWithIndex)
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
      },

      selectedItems () {
        var selectedItems = []
        this.selected.forEach(item => {
          var itemSelected = this.$paper.grid.getItems()[item._indexRowItemTable]
          selectedItems.push(itemSelected)
        })
        return selectedItems
      }
    },
    watch: {
      selected () {
        this.$store.commit('itemsSelected', this.selectedItems)
      }
    }
  }
</script>
