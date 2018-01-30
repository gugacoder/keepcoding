<template lang="pug">
    v-dialog(
      v-model="actionBarForm"
      scrollable
      max-width="1000"
    )
      v-card
        v-card-title(primary-title)
          h2 {{ $_actionsMixin_getActionTitle(action) }}
        v-card-text
          v-form(
            v-if="action"
            :ref="formName"
          )
            v-layout(
              row 
              wrap 
              v-for="field in fields" 
              :key="field.name"
            )
              v-flex(xs12)
                component(
                  :is="$_formsMixin_dynamicComponent(field)"
                  :field="field"
                )
        
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
            @click.stop="close"
          ) Fechar
</template>

<script>
  import FormsMixin from '../mixins/FormsMixin.js'
  import ActionsMixin from '../mixins/ActionsMixin.js'
  import RequestMixin from '../mixins/RequestMixin.js'
  export default {
    data: () => ({
      actionBarForm: false,
      action: null
    }),
    mixins: [
      FormsMixin,
      ActionsMixin,
      RequestMixin
    ],
    computed: {
      fields () {
        var selectedItems = this.$store.state.selection.itemsSelected
        var fields = this.$_actionsMixin_getActionsField(selectedItems, this.action.name)
        return fields
      },

      formName () {
        return 'form-' + this.action.name
      }
    },
    methods: {
      submit () {
        var queryParams = this.$_formsMixin_makeParams(this.action.name)
        this.$_requestMixin_httpRequest(this.action.method, this.action.href, queryParams).then(response => {
          if (response.ok) {
            this.$notify({ message: 'Operação realizada com sucesso!', type: 'success' })
            this.close()
          }
        })
      },

      escapeKeyListener (event) {
        if (event.keyCode === 27) {
          this.close()
        }
      },

      open (action) {
        this.action = action
        this.actionBarForm = true
      },

      close () {
        this.actionBarForm = false
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
