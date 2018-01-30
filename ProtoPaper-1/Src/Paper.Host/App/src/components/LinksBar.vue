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
        v-for="link in $store.getters.links" 
        :key="link.href"
        :target="$_routerMixin_target(link)"
        @click.stop="$_routerMixin_request(link.href)"
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

    v-divider(v-if="$store.getters.showActions")
    
    v-list(
      subheader 
      v-if="$store.getters.showActions"
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
            v-html="$_actionsMixin_getActionTitle(action)"
          )
</template>

<script>
  import ActionsMixin from '../mixins/ActionsMixin.js'
  import RouterMixin from '../mixins/RouterMixin.js'
  export default {
    mixins: [
      ActionsMixin,
      RouterMixin
    ],
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      show () {
        var show = !this.$store.state.selection.selectionState &&
                   (this.$store.getters.showLinks || this.$store.getters.showActions)
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
