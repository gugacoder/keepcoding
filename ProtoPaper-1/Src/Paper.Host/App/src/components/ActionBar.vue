<template lang="pug">
  v-toolbar(
    color="white"
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
      v-for="action in actions" 
      :key="action.name"
      @click.stop="actionClick(action)"
    ) {{ $_actionsMixin_getActionTitle(action) }}


    action-bar-form(ref="actionBarForm")
</template>

<script>
  import { Events } from '../event-bus.js'
  import parser from '../paper/parser.js'
  import ActionsMixin from '../mixins/ActionsMixin.js'
  import ActionBarForm from './ActionBarForm.vue'
  export default {
    mixins: [
      ActionsMixin
    ],
    components: {
      ActionBarForm
    },
    data: () => ({
      actionName: ''
    }),
    computed: {
      showClass () {
        if (!this.$store.state.selection.selectState) {
          return 'display: none'
        }
      },

      actions () {
        var selectedItems = this.$store.state.selection.itemsSelected
        var actions = parser.methods.getActions(selectedItems)
        return actions && actions.length > 0 ? actions : []
      },

      action () {
        var action = this.actions.filter(a => a.name === this.actionName)[0]
        return action
      }
    },
    methods: {
      deselected () {
        Events.$emit('selectState', false)
        this.$store.commit('selectState', false)
      },

      actionClick (action) {
        this.openDialog(action)
        this.actionName = action.name
      },

      openDialog (action) {
        this.$refs.actionBarForm.open(action)
      },

      closeDialog () {
        this.$refs.actionBarForm.close()
      }
    }
  }
</script>
