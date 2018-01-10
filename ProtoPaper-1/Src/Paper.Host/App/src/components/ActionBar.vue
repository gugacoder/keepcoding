<template lang="pug">
  v-toolbar(
    color="amber lighten-2"
    :class="showClass"
    fixed
  )
    v-btn(
      icon 
      @click="deselected"
    )
      v-icon arrow_back
    
    v-toolbar-title
      | Cancelar

    v-spacer

    v-btn(icon)
      v-icon search

    v-btn(icon)
      v-icon apps
    
    v-btn(icon)
      v-icon refresh

    v-btn(
      icon
    )
      v-icon more_vert

</template>

<script>
  import { Events } from '../event-bus.js'
  export default {
    computed: {
      showClass () {
        if (!this.$store.state.selection.selectState) {
          return 'display: none'
        }
      }
    },
    methods: {
      deselected () {
        Events.$emit('selectState', false)
        this.$store.commit('selectState', false)
      }
    }
  }
</script>
