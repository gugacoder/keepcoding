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
            ) {{ $paper.actions.getTitle(action) }}

            v-btn(
              color="primary"
              flat 
              @click="$_formsMixin_clear(action.name)"
            ) Limpar
</template>

<script>
  import FormsMixin from '../mixins/FormsMixin.js'
  export default {
    mixins: [
      FormsMixin
    ],
    computed: {
      action () {
        if (this.$store.state.entity && this.$store.state.entity.actions) {
          var action = this.$paper.actions.getAction(this.actionName)
          return action
        }
        return {}
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
        this.$paper.requester.httpRequest(this.action.method, this.action.href, queryParams).then(response => {
          if (response.ok) {
            this.$notify({ message: 'Operação realizada com sucesso!', type: 'success' })
            var location = response.data.headers.get('Location')
            if (location && location.length > 0) {
              this.$paper.requester.request(location)
            } else {
              this.$paper.requester.goToIndex()
            }
          }
        })
      }
    }
  }
</script>
