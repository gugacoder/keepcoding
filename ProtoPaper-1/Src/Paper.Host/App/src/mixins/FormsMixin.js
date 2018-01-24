import VPaperText from '../components/paper/VPaperText.vue'
import VPaperNumber from '../components/paper/VPaperNumber.vue'
import VPaperCheckbox from '../components/paper/VPaperCheckbox.vue'
import VPaperHidden from '../components/paper/VPaperHidden.vue'
import VPaperDate from '../components/paper/VPaperDate.vue'
import VPaperTime from '../components/paper/VPaperTime.vue'
import VPaperSelect from '../components/paper/VPaperSelect.vue'
import VPaperSwitch from '../components/paper/VPaperSwitch.vue'
import VPaperDatetime from '../components/paper/VPaperDatetime.vue'
export default {
  components: {
    VPaperText,
    VPaperNumber,
    VPaperCheckbox,
    VPaperHidden,
    VPaperDate,
    VPaperTime,
    VPaperSelect,
    VPaperSwitch,
    VPaperDatetime
  },
  methods: {
    $_formsMixin_dynamicComponent (field) {
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
          return 'VPaperNumber'
        case 'long':
          return 'VPaperNumber'
        case 'decimal':
          return 'VPaperNumber'
        case 'double':
          return 'VPaperNumber'
        case 'float':
          return 'VPaperNumber'
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
        if (field.value !== null && field.value !== undefined) {
          var param = field.$attrs.name
          var value = field.value
          params[param] = value
        }
      })
      return params
    },
    $_formsMixin_clear (actionName) {
      var formName = 'form-' + actionName
      var form = this.$refs[formName]
      form.reset()
    }
  }
}
