<template lang="pug">
  v-card(
    color="grey lighten-4" 
    flat
  )
    v-card-title(
      primary-title
      v-if="$store.state.entity.title"
    ) 
      div
        div(
          class="headline"
        ) {{ $store.state.entity.title }}

    v-card-text
      v-container(fluid)
        
        grid-view-pagination

        v-data-table(
          v-model="selected"
          :headers="headers"
          :items="items"
          item-key="_indexRowItemTable"
          hide-actions=true
          :select-all="hasActions"
          no-data-text="Não existem dados para exibir"
        )
          template(
            slot="items" 
            slot-scope="items"
          )

            v-checkbox(
              v-if="hasActions"
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
                        @click.stop="$_requestMixin_request(item.href)"
                      ) {{ item.title ? item.title : item.rel[0] }}

</template>

<script>
  import { Events } from '../event-bus.js'
  import RequestMixin from '../mixins/RequestMixin.js'
  import PaperMixin from '../mixins/PaperMixin.js'
  import GridViewPagination from './GridViewPagination.vue'
  export default {
    mixins: [
      RequestMixin,
      PaperMixin
    ],
    components: {
      GridViewPagination
    },
    data () {
      return {
        showLeftDrawer: '',
        showLinks: false,
        selected: [],
        bottom: false
      }
    },
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
      },

      loadPageItems () {
        var link = this.$store.getters['pagination/nextLink']
        link = '/page/' + link
        if (link) {
          this.$_paperMixin_loadPage(link).then(data => {
            this.items.push(data.entities)
          })
        }
      }
    },
    computed: {
      hasActions () {
        var exist = this.$store.getters.gridItems.filter(entity => entity.hasAction())
        return exist && exist.length > 0
      },

      items () {
        var items = []
        if (this.$store.getters.gridItems) {
          this.$store.getters.gridItems.forEach((item, index) => {
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
          var itemSelected = this.$store.getters.gridItems[item._indexRowItemTable]
          selectedItems.push(itemSelected)
        })
        return selectedItems
      }
    },
    watch: {
      selected () {
        this.$store.commit('itemsSelected', this.selectedItems)
      },
      bottom () {
        this.loadPageItems()
        this.bottom = false
      }
    }
  }
</script>
