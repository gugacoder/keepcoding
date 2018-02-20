import VPaperText from '../components/paper/VPaperText.vue'
import VPaperNumber from '../components/paper/VPaperNumber.vue'
import VPaperNumberInt from '../components/paper/VPaperNumberInt.vue'
import VPaperCheckbox from '../components/paper/VPaperCheckbox.vue'
import VPaperHidden from '../components/paper/VPaperHidden.vue'
import VPaperDate from '../components/paper/VPaperDate.vue'
import VPaperTime from '../components/paper/VPaperTime.vue'
import VPaperSelect from '../components/paper/VPaperSelect.vue'
import VPaperSwitch from '../components/paper/VPaperSwitch.vue'
import VPaperDatetime from '../components/paper/VPaperDatetime.vue'
import VPaperCurrency from '../components/paper/VPaperCurrency.vue'
export default {
  components: {
    VPaperText,
    VPaperNumber,
    VPaperNumberInt,
    VPaperCheckbox,
    VPaperHidden,
    VPaperDate,
    VPaperTime,
    VPaperSelect,
    VPaperSwitch,
    VPaperDatetime,
    VPaperCurrency
  },
  methods: {
    $_formsMixin_dynamicComponent (field) {
      if (field.type === 'hidden') {
        return 'VPaperHidden'
      }
      switch (field.dataType) {
        case 'date':
          return 'VPaperDate'
        case 'time':
          return 'VPaperTime'
        case 'bit':
          return 'VPaperSwitch'
        case 'bool':
          return 'VPaperSwitch'
        case 'boolean':
          return 'VPaperSwitch'
        case 'number':
          return 'VPaperNumber'
        case 'int':
          return 'VPaperNumberInt'
        case 'long':
          return 'VPaperNumberInt'
        case 'decimal':
          return 'VPaperNumber'
        case 'double':
          return 'VPaperNumber'
        case 'float':
          return 'VPaperNumber'
        case 'currency':
          return 'VPaperCurrency'
        case 'multi':
          if (field.type === 'text') {
            return 'VPaperSelect'
          }
          break
        default:
          return 'VPaperText'
      }
    },
    $_formsMixin_makeParams (actionName) {
      var params = {}
      var formName = 'form-' + actionName
      var form = this.$refs[formName]
      form.inputs.forEach((field) => {
        if (field.$attrs.value !== null && field.$attrs.value !== undefined) {
          var param = field.$attrs.name
          var value = field.$attrs.value
          params[param] = value
        }
      })
      return params
    },
    $_formsMixin_clear (actionName) {
      var formName = 'form-' + actionName
      console.log('formName', formName)
      var form = this.$refs[formName]
      console.log('form', form)
      form.reset()
    }
  }
}
