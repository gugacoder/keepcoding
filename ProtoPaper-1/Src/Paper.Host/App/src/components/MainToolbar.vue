<template lang="pug">
  v-toolbar(
    color="indigo"
    dark
    app
    clipped-right
    fixed
    :style="showClass"
  )
    v-btn(
      icon
      @click.stop="goBack()"
    )
      v-icon arrow_back

    v-toolbar-title(
      :style="$vuetify.breakpoint.smAndUp ? 'width: 300px; min-width: 50px' : 'min-width: 20px'" 
      class="ml-0 pl-3 "
    )
      span(
        :class="showTitle"
      ) 
        a(
          style="text-decoration: none; color: white"
          @click.stop="$paper.blueprint.goToIndexPage()"
        ) {{ $paper.blueprint.getProjectTitle() }}

    v-text-field(
      v-if="$paper.blueprint.showNavBox()"
      @keyup.enter="search"
      v-model="searchParams"
      clearable
      light
      solo
      prepend-icon="search"
      placeholder="Buscar Rota"
      style="max-width: 700px; min-width: 200px"
    )

    v-spacer

    v-toolbar-side-icon(
      v-if="$paper.navigation.showRightMenu()"
      @click.stop="$paper.navigation.changeRightMenuState()"
    )
</template>

<script>
  export default {
    data: () => ({
      searchParams: '',
      demoPage: '/demo'
    }),

    beforeRouteUpdate (to, from, next) {
      console.log('beforeRouteUpdate route', this.$router.history)
      next()
    },

    created () {
      console.log('created route', this.$router.history)
    },

    computed: {
      showClass () {
        if (this.$paper.state.selection) {
          return 'display: none'
        }
      },

      showTitle () {
        return this.$paper.blueprint.showNavBox() ? 'hidden-xs-only' : ''
      }
    },

    methods: {
      search () {
        this.$paper.requester.redirectToPage(this.searchParams)
      },

      clearSearch () {
        this.searchParams = ''
      },

      goBack () {
        var back = -1
        if (this.$route.name === 'notFound') {
          back = -2
        }
        this.$router.go(back)
      }
    }
  }
</script>
