<template lang="pug">
    v-footer(
      color="indigo"
    )
      span(
        class="white--text"
      )
        | KeepCoding &copy; 2017  v{{ version }}
</template>

<script>
  export default {
    computed: {
      version () {
        var pkg = require('../../package.json')
        return pkg.version
      }
    }
  }
</script>>