<template lang="pug">
  v-container(
    fluid
    fill-height
  )
    v-container(
      fluid 
      class="text-xs-center"
    )
      v-layout(
        flex 
        wrap
        row
        justify-space-between
      )
        v-flex(xs12)
          h1 {{ error.response.status }}

        v-flex(xs12)
          h2 {{ message }}
</template>

<script>
  import errors from '../paper/errors.js'
  export default {
    props: ['error'],
    computed: {
      message () {
        return errors.methods.translate(this.error.response.status)
      }
    }
  }
</script>