<template lang="pug">
  v-container(
      fluid
    )
      v-flex(
        xs12
        sm8
        offset-sm2
      )
        v-card(
          class="elevation-3"
        )
          v-card-title(primary-title)
            h2 {{ $store.state.data.title }}

          v-card-text
            v-container(fluid)
              v-flex(
                xs12 
                sm12
              )
                v-list(two-line)
                  v-list-tile(
                    v-for="item in items" 
                    :key="item.key"
                  )
                    v-list-tile-content
                      v-list-tile-title
                        | {{ item.value }}
                      v-list-tile-sub-title
                        | {{ item.key }}
</template>

<script>
  export default {
    data () {
      return {
        headers: [],
        items: []
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    methods: {
      load () {
        this.setItems()
      },
      setItems () {
        var data = this.$store.state.data
        var keys = Object.keys(data.properties)
        var self = this
        keys.forEach(function (key) {
          self.items.push({
            key: key,
            value: data.properties[key]
          })
        })
      }
    },
    created () {
      this.load()
    }
  }
</script>
