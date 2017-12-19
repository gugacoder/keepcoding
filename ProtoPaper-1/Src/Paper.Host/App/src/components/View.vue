<template>
  <v-card color="grey lighten-4" flat>
    <v-card-text>
      <v-container fluid>
        <v-layout row v-for="item in items">
          <v-flex xs12 sm10>
            <v-text-field box readonly v-bind:label="item.key" v-model="item.value"></v-text-field>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
  import { EventBus } from '../event-bus.js'
  export default {
    props: ['data'],
    data () {
      return {
        headers: [],
        items: [],
        siren: ''
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    },
    methods: {
      load () {
        this.setItems()
      },
      reset (newValue) {
        Object.assign(this.$data, this.$options.data())
        this.siren = newValue
        this.load()
      },
      setItems () {
        if (this.siren) {
          var keys = Object.keys(this.siren.properties)
          var self = this
          keys.forEach(function (key) {
            self.items.push({
              key: key,
              value: self.siren.properties[key].toString()
            })
          })
        }
      }
    },
    created () {
      EventBus.$on('reset', this.reset)
      this.siren = this.data
      if (this.siren) {
        this.load()
        return
      }
    }
  }
</script>
