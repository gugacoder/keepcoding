<template lang="pug">
  v-navigation-drawer(
    fixed
    right
    clipped
    v-model="$paper.navigation.openedRightMenu()"
    app
    v-if="show"
  )
    v-list(
      subheader
      v-if="$paper.navigation.hasLinks()"
    )
      v-subheader
        | NAVEGAÇÃO

      v-list-tile(
        v-for="link in $paper.navigation.links"
        :key="link.href"
        :target="$paper.requester.target(link)"
        @click.stop="$paper.requester.redirectToPage(link.href)"
      )
        v-list-tile-content
          v-list-tile-title(
            v-if="link.title" 
            v-html="link.title"
          )
          v-list-tile-title(
            v-else 
            v-html="link.rel[0]"
          )

    v-divider(v-if="$paper.actions.hasActions()")
    
    v-list(
      subheader 
      v-if="$paper.actions.hasActions()"
    )
      v-subheader
        | AÇÕES
      v-list-tile(
        v-for="action in $paper.getEntity().actions" 
        :key="action.name"
        @click.stop="opneActionPage(action)"
      )
        v-list-tile-content
          v-list-tile-title(
            v-html="$paper.actions.getTitle(action)"
          )

    v-divider(v-if="$paper.blueprint.hasProjectInfo()")
    
    v-list(
      subheader 
      v-if="$paper.blueprint.hasProjectInfo()"
    )
      v-subheader
        | SOBRE
      v-list-tile(
        @click.stop="openAboutPage()"
      )
        v-list-tile-content
          v-list-tile-title
            | Sobre o {{ $paper.blueprint.getProjectTitle() }}
          
</template>

<script>
  import Breakpoint from '../mixins/Breakpoint.js'
  export default {
    beforeRouteUpdate (to, from, next) {
      next()
    },

    mixins: [
      Breakpoint
    ],

    methods: {
      opneActionPage (action) {
        this.$router.push({
          name: 'form',
          params: {
            path: this.$route.params.path
          },
          query: {
            action: action.name
          }
        })
      },

      openAboutPage () {
        this.$router.push({ name: 'about' })
      }
    },

    computed: {
      show () {
        var show = !this.$paper.state.selection &&
                   this.$paper.navigation.openedRightMenu() &&
                   this.$paper.navigation.showRightMenu()
        return show
      }
    }
  }
</script>
