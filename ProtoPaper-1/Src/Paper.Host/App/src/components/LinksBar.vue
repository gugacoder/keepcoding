<template lang="pug">
  v-navigation-drawer(
    fixed
    right
    clipped
    v-model="$store.state.navigation.show"
    app
    v-if="show"
  )
    v-list(
      subheader v-if="showLinks"
    )
      v-subheader
        | NAVEGAÇÃO
      v-list-tile(
        v-for="item in $store.state.data.links" 
        v-if="item.rel.indexOf('self')"
        :key="item.href" 
        :href="item.href"
      )
        v-list-tile-content
          v-list-tile-title(
            v-if="item.title" 
            v-html="item.title"
          )
          v-list-tile-title(
            v-else v-html="item.rel[0]"
          )

    v-divider
    
    v-list(
      subheader 
      v-if="showActions"
    )
      v-subheader
        | AÇÕES
      v-list-tile(
        v-for="item in $store.state.data.actions" 
        :key="item.name" 
        :href="'/#' + $route.path + '?actions=' + item.name"
      )
        v-list-tile-content
          v-list-tile-title(
            v-html="item.title"
          )
</template>

<script>
  export default {
    beforeRouteUpdate (to, from, next) {
      next()
    },
    computed: {
      show () {
        var show = !this.$store.state.selection.selectedMode && (this.showLinks || this.showActions)
        return show
      },
      showLinks () {
        var show = this.$store.state.data && this.$store.state.data.links
        return show
      },
      showActions () {
        var show = this.$store.state.data && this.$store.state.data.actions
        return show
      }
    }
  }
</script>
