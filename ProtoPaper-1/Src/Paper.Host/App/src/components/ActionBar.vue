<template lang="pug">
  v-toolbar(
    dark 
    color="indigo"
    :class="showClass"
    fixed
  )

    v-btn(
      icon 
      @click="deselected"
    )
      v-icon arrow_back
    
    v-toolbar-title(class="white--text")
      | Cancelar

    v-spacer

    v-btn(icon)
      v-icon search

    v-btn(icon)
      v-icon apps
    
    v-btn(icon)
      v-icon refresh

    v-btn(icon)
      v-icon more_vert
</template>

<script>
  import { Events } from '../event-bus.js'
  export default {
    computed: {
      showClass () {
        var show = this.$store.state.selectedMode ? 'hidden-sm-and-down' : 'hidden-sm-and-up'
        return show
      }
    },
    methods: {
      deselected () {
        Events.$emit('selectMode', false)
        this.$store.commit('selectMode', false)
      }
    }
  }
</script>
