<template lang="pug">
  v-card(
    color="grey lighten-4" 
    flat
  )
    v-card-text
      v-container(fluid)
        v-layout(row)
          v-flex(xs12)
            v-text-field(
              name="input-1"
              label="Json"
              textarea
              rows="15"
              v-model="jsonData"
            )
        v-layout(row)
          v-flex(xs12)
            v-btn(
              color="primary" 
              flat 
              @click.native="save"
            ) Salvar
</template>

<script>
  import paper from '../paper/paper.js'
  export default {
    props: {
      routerName: String
    },
    data () {
      return {
        jsonData: ''
      }
    },
    methods: {
      save () {
        var json = this.jsonData
        if (json) {
          paper.methods.save(this.routerName, json)
        }
      }
    }
  }
</script>
