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
                        @click.stop="$_routerMixin_request(item.href)"
                      ) {{ item.title ? item.title : item.rel[0] }}

        v-flex(
          xs12
          right
        )
          v-btn(
            v-if="$store.getters['pagination/showFirst']"
            color="blue-grey"
            dark
            @click.stop="goToPage('first')"
          )
            v-icon first_page
            span Primeira Página

          v-btn(
            v-if="$store.getters['pagination/showPrevious']"
            color="blue-grey"
            dark
            @click.stop="goToPage('previous')"
          )
            v-icon navigate_before
            span Anterior
            
          v-btn(
            v-if="$store.getters['pagination/showNext']"
            color="blue-grey"
            dark
            @click.stop="goToPage('next')"
          )
            v-icon navigate_next
            span Próxima

</template>

<script>
  import { Events } from '../event-bus.js'
  import RouterMixin from '../mixins/RouterMixin.js'
  var count = 0
  export default {
    mixins: [
      RouterMixin
    ],
    data () {
      return {
        showLeftDrawer: '',
        showLinks: false,
        data: '',
        selected: [],
        busy: false
      }
    },
    created () {
      Events.$on('selectState', this.selectedMode)
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

      loadMore () {
        this.busy = true
        setTimeout(() => {
          for (var i = 0, j = 10; i < j; i++) {
            this.items.push({ name: count++ })
          }
          this.busy = false
        }, 1000)
      },

      goToPage (page) {
        var link = this.$store.getters[`pagination/${page}Link`]
        this.$_routerMixin_request(link)
      }
    },
    computed: {
      validEntities () {
        if (this.$store.state.entity && this.$store.state.entity.hasSubEntityByClass('item')) {
          return this.$store.state.entity.getSubEntitiesByClass('item')
        }
        return []
      },

      hasActions () {
        var exist = this.validEntities.filter(entity => entity.hasAction())
        return exist && exist.length > 0
      },

      items () {
        var items = []
        if (this.validEntities) {
          this.validEntities.forEach((item, index) => {
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
          var itemSelected = this.validEntities[item._indexRowItemTable]
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
