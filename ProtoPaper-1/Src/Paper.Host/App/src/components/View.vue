<template>
  <v-card color="grey lighten-4" flat>
    <v-card-text>
      <v-container fluid>
        <v-layout row v-for="item in items" :key="item.key">
          <v-flex xs12 sm10>
            <v-text-field box readonly v-bind:label="item.key" v-model="item.value"></v-text-field>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
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
      this.$store.dispatch('reloadAsync').then(() => {
        this.load()
      })
    }
  }
</script>
