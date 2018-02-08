<template lang="pug">
  v-navigation-drawer(
    fixed
    right
    clipped
    :temporary="isMobile"
    v-model="$store.state.navigation.show"
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
        @click.stop="$paper.requester.request(link.href)"
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
        @click.stop="push(action)"
      )
        v-list-tile-content
          v-list-tile-title(
            v-html="$paper.actions.getTitle(action)"
          )
</template>

<script>
  export default {
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      show () {
        var show = !this.$store.state.selection.selectionState &&
                   (this.$paper.navigation.hasLinks() || this.$paper.actions.hasActions)
        return show
      },

      isMobile () {
        return window.innerWidth < 993
      }
    },
    methods: {
      push (action) {
        this.$router.push({ query: { action: action.name } })
      }
    }
  }
</script>
