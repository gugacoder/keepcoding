<template lang="pug">
  v-card
    v-container(
      fluid
      grid-list-md
    )
      div(v-if="$paper.page.hasTitle()")
        div(class="headline") 
          | {{ $paper.page.title }}

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
            v-for="(header, index) in headers"
            :key="items.index.toString() + index.toString()"
            class="text-xs-left"
            nowrap
            @click.stop="openItemView(items.item)"
          ) {{ items.item[header.value] }}

          td(
            class="text-xs-center" 
            @click.stop=""
          )
            v-menu(
              offset-x 
              left 
              bottom 
              v-if="hasItemLinks(items.index)"
            )
              v-btn(
                icon
                slot="activator"
              )
                v-icon
                  | more_vert

              v-list
                v-list-tile(
                  v-for="item in itemLinks(items.index)" 
                  :key="item.href"
                )
                  v-list-tile-content
                    a(
                      @click.stop="$paper.requester.redirectToPage(item.href)"
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
      selected: []
    }),

    created () {
      Events.$on('selectState', this.selectedMode)
    },

    beforeRouteLeave (to, from, next) {
      this.$paper.state.disableSelectionState()
      next()
    },

    methods: {
      columnKey (column, index, item) {
        return column.value
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

      openItemView (item) {
        var entireItem = this.$paper.grid.validItems[item._indexRowItemTable]
        var link = entireItem.getLinkByRel('self')
        this.$paper.requester.redirectToPage(link.href)
      },

      itemLinks (index) {
        var entity = this.$paper.getEntity().entities[index]
        var links = entity.links.filter((link) => !link.rel.includes('self'))
        return links
      },

      hasItemLinks (index) {
        var items = this.itemLinks(index)
        return items && items.length > 0
      }
    },

    computed: {
      items () {
        return this.$paper.grid.items
      },

      headers () {
        return this.$paper.grid.headers
      },

      selectedItems () {
        var selectedItems = []
        this.selected.forEach(item => {
          var itemSelected = this.$paper.grid.validItems[item._indexRowItemTable]
          selectedItems.push(itemSelected)
        })
        return selectedItems
      }
    },

    watch: {
      selected () {
        this.$paper.grid.setSelectedItems(this.selectedItems)
      }
    }
  }
</script>
