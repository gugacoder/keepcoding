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
          h2 {{ action.title }}

        v-card-text
          v-form(
            v-model="valid"
            :ref="'form-' + action.name"
            lazy-validation
          )
            v-layout(
              row 
              wrap 
              v-for="field in action.fields" 
              :key="field.name"
            )

              v-flex(xs12)
                component(:is="dynamicComponent(field)" :field="field")

            v-btn(@click="submit()")
              | {{ action.title }}
            v-btn(@click="clear()")
              | Limpar
</template>

<script>
  import TextView from './types/TextView.vue'
  import NumberView from './types/NumberView.vue'
  import CheckboxView from './types/CheckboxView.vue'
  import HiddenView from './types/HiddenView.vue'
  import DateView from './types/DateView.vue'
  import TimeView from './types/TimeView.vue'
  import SelectView from './types/SelectView.vue'
  import SwitchView from './types/SwitchView.vue'
  import DatetimeView from './types/DatetimeView.vue'
  import paper from '../paper/paper.js'
  export default {
    $validates: true,
    data: () => ({
      valid: true
    }),
    beforeRouteUpdate (to, from, next) {
      next()
    },
    components: {
      TextView,
      NumberView,
      CheckboxView,
      HiddenView,
      DateView,
      TimeView,
      DatetimeView,
      SelectView,
      SwitchView
    },
    computed: {
      action () {
        if (this.$store.state.data.actions) {
          var actionName = this.$route.query.actions
          return this.$store.state.data.getActionByName(actionName)
        }
        return []
      }
    },
    methods: {
      dynamicComponent (field) {
        switch (field.dataType) {
          case 'date':
            return 'DateView'
          case 'time':
            return 'TimeView'
          case 'bit':
            return 'SwitchView'
          case 'bool':
            return 'SwitchView'
          case 'boolean':
            return 'SwitchView'
          case 'number':
            return 'NumberView'
          case 'int':
            return 'NumberView'
          case 'long':
            return 'NumberView'
          case 'decimal':
            return 'NumberView'
          case 'double':
            return 'NumberView'
          case 'float':
            return 'NumberView'
          case 'multi':
            if (field.type === 'text') {
              return 'SelectView'
            }
            break
          default:
            return 'TextView'
        }
      },
      submit () {
        var queryParams = this.makeParams()
        var method = this.action.method.toLowerCase()
        this.$http[method](this.action.href, queryParams).then(response => {
          var location = response.headers.get('Location')
          if (location && location.length > 0) {
            paper.methods.load(location)
          } else {
            this.$router.go(-1)
          }
        }, response => {
          console.log('error ', response)
          this.$router.go(-1)
        })
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
