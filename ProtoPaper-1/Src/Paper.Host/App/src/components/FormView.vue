<template lang="pug">
  v-container(
    fluid
  )
    v-flex(
      xs12
      sm8
      offset-sm2
    )
      v-card(
        class="elevation-3"
      )
        v-card-title(primary-title)
          h2 {{ action ? action.title : '' }}

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

            v-btn(
              color="primary"
              @click="submit()"
            ) {{ $_actionsMixin_getActionTitle(action) }}

            v-btn(
              color="primary"
              flat 
              @click="$_formsMixin_clear(actionName)"
            ) Limpar
</template>

<script>
  import FormsMixin from '../mixins/FormsMixin.js'
  import ActionsMixin from '../mixins/ActionsMixin.js'
  import RequestMixin from '../mixins/RequestMixin.js'
  export default {
    mixins: [
      FormsMixin,
      ActionsMixin,
      RequestMixin
    ],
    computed: {
      action () {
        if (this.$store.state.entity.actions) {
          var action = this.$store.state.entity.getActionByName(this.actionName)
          return action
        }
      },

      fields () {
        return this.action ? this.action.fields : []
      },

      actionName () {
        return this.$route.query.action
      }
    },
    methods: {
      submit () {
        var queryParams = this.$_formsMixin_makeParams(this.actionName)
        this.$_requestMixin_httpRequest(this.action.method, this.action.href, queryParams).then(response => {
          if (response.ok) {
            this.$notify({ message: 'Operação realizada com sucesso!', type: 'success' })
            var location = response.data.headers.get('Location')
            if (location && location.length > 0) {
              this.$_requestMixin_request(location)
            } else {
              this.$_requestMixin_goToIndex()
            }
          }
        })
      }
    }
  }
</script>
