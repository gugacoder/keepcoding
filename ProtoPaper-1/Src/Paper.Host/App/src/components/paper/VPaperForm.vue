<template lang="pug">
  v-form(
    v-if="action"
    :ref="formName"
  )
    v-container(
      fluid
      grid-list-xs
    )
      v-layout(
        row
        v-for="field in fields" 
        :key="field.name"
      )
        v-flex(xs12)
          component(:is="$_formsMixin_dynamicComponent(field)" :field="field")


    v-btn(
      color="secondary"
      @click="submit()"
    ) {{ action.title }}

    v-btn(
      color="white"
      @click="$_formsMixin_clear(action.name)"
    ) Limpar

</template>

<script>
  import FormsMixin from '../../mixins/FormsMixin.js'
  export default {
    props: ['action'],

    mixins: [
      FormsMixin
    ],

    computed: {
      fields () {
        return this.action ? this.action.fields : []
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
            this.$notify({ message: 'Operação realizada com sucesso!', type: 'success' })
            var location = response.data.headers['Location']
            if (location && location.length > 0) {
              this.$paper.requester.redirectToPage(location)
            } else {
              this.$paper.requester.goToRootPage()
            }
          } else {
            var error = this.$paper.errors.httpTranslate(response.data.status)
            var message = 'Erro ao acessar a url: ' + this.action.href + '. ' + error
            this.$notify({ message: message, type: 'danger' })
          }
        })
      }
    }

  }
</script>

<style lang="sass">
  .input-group--text-field
    height: 50px
</style>
