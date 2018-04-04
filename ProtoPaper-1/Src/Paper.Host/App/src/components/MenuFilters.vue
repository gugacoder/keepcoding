<template lang="pug">
  v-navigation-drawer(
    fixed
    left
    v-model="$store.state.filters.openedFiltersMenu"
    clipped
    app
    v-if="show"
    class="grey lighten-4"
    style="max-height: calc(100% - 60px)"
  )
    v-subheader APLICAR FILTROS

    v-container(
      fluid
      class="nobordertop"
    )
      v-layout(
        flex
        row
        justify-space-between
      )
        v-flex(xs12)
          paper-form(
            :action="$paper.filters.entity"
          )

</template>

<script>
  import FormsMixin from '../mixins/FormsMixin.js'
  import PaperForm from './paper/VPaperForm.vue'
  export default {
    beforeRouteUpdate (to, from, next) {
      next()
    },

    mixins: [
      FormsMixin
    ],

    methods: {
      openActionPage (action) {
        var params = this.$route.params.path
        this.$paper.requester.redirectToForm(params, action.name)
      },

      openAboutPage () {
        this.$router.push({ name: 'about' })
      }
    },

    components: {
      PaperForm
    },

    computed: {
      show () {
        var show = !this.$paper.state.selection &&
                   this.$store.state.filters.openedFiltersMenu &&
                   !this.$paper.isFormPage(this.$route.name)
        return show
      }
    }
  }
</script>

<style lang="sass">
  .nobordertop
    padding-top: 0px

  .noborderbottom  
    padding-bottom: 0px
</style>
