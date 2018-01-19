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
      @click.stop="actionClick(action)"
    ) {{ action ? action.title : ''}}

    v-dialog(
      v-model="dialogActionBarForm"
      scrollable
      max-width="1000"
    )

      v-card
        v-card-title(primary-title)
          h2 {{ action ? action.title : '' }}

        v-card-text
          v-form(
            v-if="action"
            v-model="valid"
            :ref="'form-' + action.name"
            lazy-validation
          )
            v-layout(
              row 
              wrap 
              v-for="field in fields" 
              :key="field.name"
            )

              v-flex(xs12)
                component(:is="dynamicComponent(field)" :field="field")
        
        v-divider

        v-card-actions
          v-btn(
            color="primary"
            flat
            @click="submit()"
          ) {{ action ? action.title : '' }}

          v-btn(
            color="primary"
            flat
            @click="clear()"
          ) Limpar

          v-btn(
            color="primary"
            flat
            @click.stop="closeDialog"
          ) Fechar

</template>

<script>
  import { Events } from '../event-bus.js'
  import parser from '../paper/parser.js'
  import FormsMixin from '../mixins/FormsMixin.js'
  export default {
    mixins: [FormsMixin],
    data: () => ({
      dialogActionBarForm: false
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
        return actions
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
        var queryParams = this.makeParams()
        var method = this.action.method.toLowerCase()
        this.$http[method](this.action.href, queryParams).then(response => {
          this.closeDialog()
        }, response => {
          console.log('error ', response)
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
