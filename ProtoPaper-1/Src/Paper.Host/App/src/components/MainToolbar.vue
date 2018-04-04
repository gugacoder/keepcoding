<template lang="pug">
  v-toolbar(
    color="primary"
    dark
    app
    clipped-left
    clipped-right
    fixed
    :style="showClass"
  )
    v-btn(
      icon
      @click.stop="goBack()"
    )
      v-icon arrow_back

    v-btn(
      icon
      v-if="showFiltersMenu"
      @click.stop="$paper.filters.changeMenuState()"
    ) 
      v-icon filter_list

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

    v-menu(
      offset-y
      v-if="$paper.auth.isAuthenticated"
    )
      v-btn(
        icon
        slot="activator"
      )
        v-icon format_color_fill

      v-list
        v-list-tile(@click="changeTheme('red')")
          v-list-tile-avatar
            v-icon(color="red") lens
          v-list-tile-title Vermelho
        v-list-tile(@click="changeTheme('black')")
          v-list-tile-avatar
            v-icon(color="black") lens
          v-list-tile-title Preto
        v-list-tile(@click="changeTheme('purple')")
          v-list-tile-avatar
              v-icon(color="purple") lens
          v-list-tile-title Roxo
        v-list-tile(@click="changeTheme('pink')")
          v-list-tile-avatar
              v-icon(color="pink") lens
          v-list-tile-title Rosa
        v-list-tile(@click="changeTheme('deep-purple')")
          v-list-tile-avatar
              v-icon(color="deep-purple") lens
          v-list-tile-title Lilás
        v-list-tile(@click="changeTheme('orange')")
          v-list-tile-avatar
              v-icon(color="orange") lens
          v-list-tile-title Laranja
        v-list-tile(@click="changeTheme('grey')")
          v-list-tile-avatar
            v-icon(color="grey") lens
          v-list-tile-title Cinza
        v-list-tile(@click="changeTheme('green')")
          v-list-tile-avatar
            v-icon(color="green") lens
          v-list-tile-title Verde
        v-list-tile(@click="changeTheme('indigo')")
          v-list-tile-avatar
            v-icon(color="indigo") lens
          v-list-tile-title Padrão
        v-list-tile(@click="changeTheme('blue')")
          v-list-tile-avatar
            v-icon(color="blue") lens
          v-list-tile-title Azul
        v-list-tile(@click="changeTheme('cyan')")
          v-list-tile-avatar
            v-icon(color="cyan") lens
          v-list-tile-title Ciano                      

    v-menu(
      offset-y
      v-if="$paper.auth.isAuthenticated"
    )
      v-btn(
        icon
        slot="activator"
      )
        v-icon person

      v-list
        v-list-tile(@click="$paper.auth.logout()")
          v-list-tile-title Logout

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
      next()
    },

    computed: {
      showClass () {
        if (this.$paper.state.selection) {
          return 'display: none'
        }
      },

      showTitle () {
        return this.$paper.blueprint.showNavBox() ? 'hidden-xs-only' : ''
      },

      showFiltersMenu () {
        return this.$paper.filters.hasFilters() && !this.$paper.isFormPage(this.$route.name)
      }
    },

    methods: {
      changeTheme (theme) {
        this.$paper.blueprint.setPredefinedTheme(theme)
      },

      search () {
        this.$paper.requester.redirectToPage(this.searchParams)
      },

      clearSearch () {
        this.searchParams = ''
      },

      goBack () {
        this.$router.go(-1)
      },

      showUserMenu () {
        this.$paper.user.changeMenuState()
      }
    }
  }
</script>
