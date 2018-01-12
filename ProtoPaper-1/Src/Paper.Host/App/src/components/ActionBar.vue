<template lang="pug">
  v-toolbar(
    color="amber lighten-2"
    :class="showClass"
    fixed
  )
    v-btn(
      icon 
      @click="deselected"
    )
      v-icon arrow_back
    
    v-toolbar-title
      | Cancelar

    v-spacer

    v-subheader(name="itensSelecionadosLabel")
      | {{ $store.state.selection.itemsSelected.length }} 
      | {{ $store.state.selection.itemsSelected.length > 1 ? 'itens selecionados' : 'item selecionado'}} 

    v-btn(
      flat
      v-if="actions && actions.length > 0"
      v-for="action in actions" 
      :key="action.name"
      :href="'/#' + $route.path + '?actions=' + action.name"
    ) {{ action ? action.title : ''}}

</template>

<script>
  import { Events } from '../event-bus.js'
  import parser from '../paper/parser.js'
  export default {
    computed: {
      showClass () {
        if (!this.$store.state.selection.selectState) {
          return 'display: none'
        }
      },
      actions () {
        var selectedItems = this.$store.state.selection.itemsSelected
        var actions = parser.methods.getActions(selectedItems)
        return actions
      }
    },
    methods: {
      deselected () {
        Events.$emit('selectState', false)
        this.$store.commit('selectState', false)
      }
    }
  }
</script>
