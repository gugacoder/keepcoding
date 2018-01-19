<template lang="pug">
  v-alert(
    color="info"
    :icon="info"
    dismissible
    v-model="alert"
    transition="scale-transition"
  ) {{ message }}
</template>

<script>
  export default {
    props: [alert, message],
    computed: {
      icon () {
        switch(alert) {
          case 'error':
            return 'warning'
          case 'info':
            return 'info'
          case 'warning':
            return 'priority_high'
          default:
            return 'check_circle'
        }
      }
    }
  }
</script>
