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
      v-for="action in actions" 
      :key="action.name"
      @click.stop="actionClick(action)"
    ) {{ $_actionsMixin_getActionTitle(action) }}

    v-dialog(
      v-model="dialogActionBarForm"
      scrollable
      max-width="1000"
    )

      v-card
        v-card-title(primary-title)
          h2 {{ $_actionsMixin_getActionTitle(action) }}

        v-card-text
          v-form(
            v-if="action"
            :ref="'form-' + action.name"
          )
            v-layout(
              row 
              wrap 
              v-for="field in fields" 
              :key="field.name"
            )

              v-flex(xs12)
                component(:is="$_formsMixin_dynamicComponent(field)" :field="field")
        
        v-divider

        v-card-actions
          v-btn(
            color="primary"
            flat
            @click="submit()"
          ) {{ $_actionsMixin_getActionTitle(action) }}

          v-btn(
            color="primary"
            flat
            @click="$_formsMixin_clear(actionName)"
          ) Limpar

          v-btn(
            color="primary"
            flat
            @click.stop="closeDialog"
          ) Fechar

</template>

<script>
  import { Events } from '../event-bus.js'
  import requester from '../paper/requester.js'
  import parser from '../paper/parser.js'
  import errors from '../paper/errors.js'
  import FormsMixin from '../mixins/FormsMixin.js'
  import ActionsMixin from '../mixins/ActionsMixin.js'
  export default {
    mixins: [
      FormsMixin,
      ActionsMixin
    ],
    data: () => ({
      dialogActionBarForm: false,
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
      },

      fields () {
        var selectedItems = this.$store.state.selection.itemsSelected
        var fields = parser.methods.getActionsField(selectedItems, this.actionName)
        return fields
      }
    },
    methods: {
      deselected () {
        Events.$emit('selectState', false)
        this.$store.commit('selectState', false)
      },

      actionClick (action) {
        this.openDialog()
        this.actionName = action.name
      },

      submit () {
        var queryParams = this.$_formsMixin_makeParams(this.actionName)
        requester.methods.request(this.action.method, this.action.href, queryParams).then(response => {
          if (!response.ok) {
            var message = errors.methods.translate(response.response.statusText)
            this.$notify({ message: message, type: 'danger' })
            return
          }
          this.closeDialog()
        })
      },

      escapeKeyListener (event) {
        if (event.keyCode === 27) {
          this.closeDialog()
        }
      },

      openDialog () {
        this.dialogActionBarForm = true
      },

      closeDialog () {
        this.dialogActionBarForm = false
        this.$refs.alert.hide()
      }
    },
    created () {
      document.addEventListener('keyup', this.escapeKeyListener)
    },
    destroyed () {
      document.removeEventListener('keyup', this.escapeKeyListener)
    }
  }
</script>
