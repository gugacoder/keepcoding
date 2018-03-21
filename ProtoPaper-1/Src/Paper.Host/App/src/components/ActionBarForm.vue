<template lang="pug">
    v-dialog(
      v-model="actionBarForm"
      scrollable
      max-width="1000"
    )
      v-card
        v-card-title(primary-title)
          h2 {{ $paper.actions.getTitle(action) }}

          v-spacer(v-if="$breakpoint.xs")
          
          v-menu(
            bottom
            left
            v-if="$breakpoint.xs"
          )
            v-btn(
              icon
              slot="activator"
            )
              v-icon more_vert
            
            v-list
              v-list-tile(
                @click.stop="$_formsMixin_clear(action.name)"
              ) Limpar

              v-list-tile(
                @click.stop="close"
              ) Fechar
            
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
          ) {{ $paper.actions.getTitle(action) }}

          v-btn(
            color="primary"
            flat
            v-if="!$breakpoint.xs"
            @click="$_formsMixin_clear(action.name)"
          ) Limpar

          v-btn(
            color="primary"
            flat
            v-if="!$breakpoint.xs"
            @click.stop="close"
          ) Fechar
</template>

<script>
  import FormsMixin from '../mixins/FormsMixin.js'
  import Breakpoint from '../mixins/Breakpoint.js'
  export default {
    data: () => ({
      actionBarForm: false,
      action: null
    }),

    mixins: [
      FormsMixin,
      Breakpoint
    ],

    computed: {
      fields () {
        var selectedItems = this.$paper.grid.getSelectedItems()
        var fields = this.$paper.actions.getActionFields(selectedItems, this.action.name)
        return fields
      },

      formName () {
        return 'form-' + this.action.name
      }
    },

    methods: {
      submit () {
        var queryParams = this.$_formsMixin_makeParams(this.action.name)
        this.$paper.requester.httpRequest(this.action.method, this.action.href, queryParams).then(response => {
          if (response.ok) {
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
