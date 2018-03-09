<template lang="pug">
  v-navigation-drawer(
    fixed
    right
    clipped
    v-model="$store.state.navigation.openedRightMenu"
    app
    v-if="show"
  )
    v-list(subheader)
      v-subheader
        | NAVEGAÇÃO

      v-list-tile(
        v-for="link in $paper.navigation.links()"
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
        v-for="action in $store.state.entity.actions" 
        :key="action.name"
        @click.stop="pushAction(action)"
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
        @click.stop="pushAboutPage()"
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
      pushAction (action) {
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

      pushAboutPage () {
        this.$router.push({ name: 'about' })
      }
    },

    computed: {
      show () {
        var show = !this.$store.state.selection.selectionState &&
                   this.$store.state.navigation.openedRightMenu
        return show
      }
    }
  }
</script>
