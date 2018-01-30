<template lang="pug">
  v-flex(
    xs12
    right
  )
    v-btn(
      color="primary"
      flat
      :disabled="!$store.getters['pagination/showFirst']"
      @click.stop="goToPage('first')"
    )
      v-icon first_page
      span Primeira Página

    v-btn(
      :disabled="!$store.getters['pagination/showPrevious']"
      color="primary"
      flat
      @click.stop="goToPage('previous')"
    )
      v-icon navigate_before
      span Anterior
      
    v-btn(
      :disabled="!$store.getters['pagination/showNext']"
      color="primary"
      flat
      @click.stop="goToPage('next')"
    )
      v-icon navigate_next
      span Próxima
</template>


<script>
  import RouterMixin from '../mixins/RouterMixin.js'
  export default {
    mixins: [
      RouterMixin
    ],
    methods: {
      goToPage (page) {
        var link = this.$store.getters[`pagination/${page}Link`]
        this.$_routerMixin_request(link)
      }
    }
  }
</script>
