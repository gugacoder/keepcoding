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
        v-for="link in links" 
        :key="link.href"
        :target="target(link)"
        @click.stop="request(link.href)"
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

    v-divider(v-if="showActions")
    
    v-list(
      subheader 
      v-if="showActions"
    )
      v-subheader
        | AÇÕES
      v-list-tile(
        v-for="action in $store.state.data.actions" 
        :key="action.name" 
        @click.stop="push(action)"
      )
        v-list-tile-content
          v-list-tile-title(
            v-html="action.title"
          )
</template>

<script>
  import paper from '../paper/paper.js'
  export default {
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      show () {
        var show = !this.$store.state.selection.selectionState && (this.showLinks || this.showActions)
        return show
      },

      showLinks () {
        var show = this.$store.state.data && this.$store.state.data.links && this.$store.state.data.links.length > 1
        return show
      },

      showActions () {
        var show = this.$store.state.data && this.$store.state.data.actions
        return show
      },

      isMobile () {
        return window.innerWidth < 993
      },

      links () {
        var items = this.$store.state.data.links.filter(
          item => item.rel.indexOf('self')
        )
        return items
      }
    },
    methods: {
      target (link) {
        if (link.type !== undefined && !link.type.match(/json/g)) {
          return '_blank'
        }
        return '_self'
      },

      push (action) {
        this.$router.push({ query: { action: action.name } })
      },

      request (link) {
        paper.methods.request(link)
      }
    }
  }
</script>
