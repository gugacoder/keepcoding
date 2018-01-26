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
  import errors from '../paper/errors.js'
  import requester from '../paper/requester.js'
  import FormsMixin from '../mixins/FormsMixin.js'
  import ActionsMixin from '../mixins/ActionsMixin.js'
  import RouterMixin from '../mixins/RouterMixin.js'
  export default {
    mixins: [
      FormsMixin,
      ActionsMixin,
      RouterMixin
    ],
    computed: {
      action () {
        if (this.$store.state.data.actions) {
          var action = this.$store.state.data.getActionByName(this.actionName)
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
        requester.methods.request(this.action.method, this.action.href, queryParams).then(response => {
          if (!response.ok) {
            var message = errors.methods.translate(response.response.statusText)
            this.$notify({ message: message, type: 'danger' })
            return
          }
          var location = response.response.headers.get('Location')
          if (location && location.length > 0) {
            this.$_routerMixin_request(location)
          } else {
            this.$_routerMixin_goToIndex()
          }
        })
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    }
  }
</script>
