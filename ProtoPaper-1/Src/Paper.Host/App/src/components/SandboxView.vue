<template>
  <v-card color="grey lighten-4" flat>
    <v-card-text>
      <v-container fluid>
        <v-layout row>
          <v-flex xs12>
            <v-text-field
              name="input-1"
              label="Json"
              textarea
              rows="15"
              v-model="jsonData">
            </v-text-field>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex xs12>
            <v-btn color="primary" flat @click.native="save">Salvar</v-btn>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
  import paper from '../paper/paper.js'
  export default {
    props: ['routerName'],
    data () {
      return {
        jsonData: ''
      }
    },
    methods: {
      save: function () {
        this.$http.post(this.routerName, this.jsonData).then(response => {
          var json = this.jsonData
          if (json) {
            paper.methods.load(json)
          }
        }, response => {
          console.log('error')
        })
      }
    }
  }
</script>
