<template>
  <v-navigation-drawer
    fixed
    v-model="drawerLeft"
    :stateless="left"
    app>
    <v-container fluid>
      <form v-for="action in actions" :key="actions[0].name">
        <v-layout row wrap v-for="field in action.fields" :key="field.name">
          <v-flex xs12>
            <component :is="field.type + 'View'" :field="field"></component>
          </v-flex>
        </v-layout>
        <v-btn @click="submit">{{ action.title }}</v-btn>
        <v-btn @click="clear">clear</v-btn>
      </form>
    </v-container>
  </v-navigation-drawer>
</template>

<script>
  import TextView from './types/TextView.vue'
  import NumberView from './types/NumberView.vue'
  import CheckboxView from './types/CheckboxView.vue'
  import RadioView from './types/RadioView.vue'
  import HiddenView from './types/HiddenView.vue'
  import { EventBus } from '../event-bus.js'
  export default {
    $validates: true,
    data: () => ({
      actions: [],
      drawerLeft: true,
      left: false
    }),
    beforeRouteUpdate (to, from, next) {
      next()
    },
    created () {
      EventBus.$on('drawerLeft', this.setLeftDrawer)
      EventBus.$on('actions', this.setActions)
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
        this.$validator.validateAll()
      },
      clear () {
      }
    }
  }
</script>
