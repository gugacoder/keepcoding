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
  import RequestMixin from '../mixins/RequestMixin.js'
  export default {
    mixins: [
      RequestMixin
    ],
    methods: {
      goToPage (page) {
        var link = this.$store.getters[`pagination/${page}Link`]
        this.$_requestMixin_request(link)
      }
    }
  }
</script>
