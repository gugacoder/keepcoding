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
            ) {{ action ? action.title : '' }}

            v-btn(
              color="primary"
              flat 
              @click="$_formsMixin_clear(actionName)"
            ) Limpar
</template>

<script>
  import paper from '../paper/paper.js'
  import requester from '../paper/requester.js'
  import FormsMixin from '../mixins/FormsMixin.js'
  export default {
    mixins: [FormsMixin],
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
            this.$router.go(-1)
            return
          }
          var location = response.headers.get('Location')
          if (location && location.length > 0) {
            paper.methods.load(location)
          } else {
            this.$router.go(-1)
          }
        })
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    }
  }
</script>
