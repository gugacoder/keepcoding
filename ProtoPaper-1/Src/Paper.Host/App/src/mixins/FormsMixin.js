import TextView from '../components/types/TextView.vue'
import NumberView from '../components/types/NumberView.vue'
import CheckboxView from '../components/types/CheckboxView.vue'
import HiddenView from '../components/types/HiddenView.vue'
import DateView from '../components/types/DateView.vue'
import TimeView from '../components/types/TimeView.vue'
import SelectView from '../components/types/SelectView.vue'
import SwitchView from '../components/types/SwitchView.vue'
import DatetimeView from '../components/types/DatetimeView.vue'
import parser from '../paper/parser.js'
export default {
  $validates: true,
  data: () => ({
    valid: true,
    actionName: ''
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
      var action = []
      var selectionMode = this.$store.state.selection.selectionState
      if (selectionMode) {
        var selectedItems = this.$store.state.selection.itemsSelected
        var actions = parser.methods.getActions(selectedItems)
        action = actions.filter(a => a.name === this.actionName)[0]
        return action
      }
      if (this.$store.state.data && this.$store.state.data.actions) {
        action = this.$store.state.data.getActionByName(this.actionName)
        return action
      }
      return action
    },
    fields () {
      var fieldsAction = []
      var selectionMode = this.$store.state.selection.selectionState
      if (selectionMode) {
        var selectedItems = this.$store.state.selection.itemsSelected
        fieldsAction = parser.methods.getActionsField(selectedItems, this.actionName)
        return fieldsAction
      }
      if (this.$store.state.data && this.$store.state.data.actions) {
        var action = this.$store.state.data.getActionByName(this.actionName)
        fieldsAction = action.fields
        return fieldsAction
      }
      return fieldsAction
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
    makeParams () {
      var params = {}
      var formName = 'form-' + this.actionName
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
      var formName = 'form-' + this.actionName
      var form = this.$refs[formName]
      form.reset()
    }
  }
}
