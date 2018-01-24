<template lang="pug">
  v-alert(
    :color="alertType"
    :icon="icon"
    :value="alertShow"
    @click.stop="hide()"
    ref="alert"
    outline
    transition="scale-transition"
  ) {{ alertMessage }}
</template>

<script>
  export default {
    data: () => ({
      alertShow: false,
      alertTimeout: null,
      alertMessage: this.message,
      alertType: this.type,
      duration: 10
    }),
    props: {
      type: String,
      message: String,
      transition: String
    },
    computed: {
      icon () {
        switch (this.type) {
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
    },
    methods: {
      show () {
        this.alertShow = true
        if (this.duration) {
          this.alertTimeout = setTimeout(() => {
            console.log('true')
            this.alertShow = false
          }, this.duration)
        }
      },

      hide () {
        this.alertShow = false
        if (this.alertTimeout) {
          clearTimeout(this.alertTimeout)
        }
      }
    }
  }
</script>
