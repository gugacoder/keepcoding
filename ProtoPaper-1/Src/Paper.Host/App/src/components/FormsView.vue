<template lang="pug">
  v-container(fluid)
    v-form(
      :ref="'form-' + action.name"
    )
      v-layout(
        row 
        wrap 
        v-for="field in action.fields" 
        :key="field.name"
      )
        v-flex(xs12)
          component(
            :is="field.type + 'View'" 
            :field="field"
          )
      v-btn(@click="submit()")
        | {{ action.title }}
      v-btn(@click="clear()")
        | Limpar
</template>

<script>
  import TextView from './types/TextView.vue'
  import NumberView from './types/NumberView.vue'
  import CheckboxView from './types/CheckboxView.vue'
  import RadioView from './types/RadioView.vue'
  import HiddenView from './types/HiddenView.vue'
  export default {
    $validates: true,
    data: () => ({
      action: [],
      drawerLeft: true,
      left: false,
      valid: true
    }),
    beforeRouteUpdate (to, from, next) {
      next()
    },
    created () {
      this.$store.dispatch('reloadAsync').then(() => {
        var actionName = this.$route.query.actions
        this.action = this.$store.state.data.getActionByName(actionName)
      })
    },
    components: {
      TextView,
      NumberView,
      CheckboxView,
      RadioView,
      HiddenView
    },
    methods: {
      setActions (actions) {
        this.actions = actions
      },
      setLeftDrawer (drawer) {
        this.drawerLeft = drawer
      },
      submit () {
        var queryParams = this.makeParams()
        if (this.action && this.action.method === 'POST') {
          var contentType = { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }
          this.$http.post(this.action.href, {params: queryParams, headers: contentType}).then(response => {
            this.$router.go(-1)
          }, response => {
            console.log('error ', response)
            this.$router.go(-1)
          })
        }
      },
      makeParams () {
        var params = {}
        var formName = 'form-' + this.action.name
        var form = this.$refs[formName]
        form.inputs.forEach((field) => {
          if (field.value !== undefined) {
            var param = field.$attrs.name
            var value = field.value
            params[param] = value
          }
        })
        return params
      },
      clear () {
        var formName = 'form-' + this.action.name
        var form = this.$refs[formName]
        form.reset()
      }
    }
  }
</script>
