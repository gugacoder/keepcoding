<template lang="pug">
  a(@click.stop="goToLink()")
    | {{ field.title }}
</template>

<script>
  export default {
    props: [
      'field'
    ],
    methods: {
      goToLink () {
        this.$paper.requester.request(this.field.value)
      }
    }
  }
</script>
