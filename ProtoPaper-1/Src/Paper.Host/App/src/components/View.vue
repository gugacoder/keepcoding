<template>
  <v-card color="grey lighten-4" flat>
    <v-card-text>
      <v-container fluid>
        <h3>Propriedades de {{ siren.class[0] }}</h3>
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
      show () {
        this.showLinks = !this.showLinks
      },
      load () {
        this.setItems()
        this.setHeaders()
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
          console.log('items ', this.items)
        }
      },
      setHeaders () {
        var item = this.items[0]
        if (item) {
          var keys = Object.keys(item)
          var self = this
          keys.forEach(function (key) {
            self.headers.push({
              text: key,
              align: 'left',
              sortable: false
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
