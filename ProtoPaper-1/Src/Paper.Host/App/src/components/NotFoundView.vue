<template>
  <v-container fluid fill-height>
    <v-container fluid class="text-xs-center">
      <v-layout flex wrap row justify-space-between>
        <v-flex xs12>
          <h1>404</h1>
        </v-flex>
        <v-flex xs12>
          <h2>Página não encontrada!</h2>
        </v-flex>
        <v-flex xs12 class="mt-5">
          <v-dialog v-model="dialog" max-width="300px">
            <v-btn color="primary" dark slot="activator">Projetar essa rota</v-btn>
            <v-card>
              <v-card-title>Selecione um módulo</v-card-title>
              <v-divider></v-divider>
              <v-card-text>
                <v-radio-group v-model="dialog1" column>
                  <v-radio label="Sandbox" value="sandbox"></v-radio>
                </v-radio-group>
              </v-card-text>
              <v-divider></v-divider>
              <v-card-actions>
                <v-btn color="primary" flat @click.native="dialog=false">Cancelar</v-btn>
                <v-btn color="primary" flat @click.native="createRouter">Ok</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-flex>
      </v-layout>
    </v-container>
  </v-container>  
</template>

<script>
  export default {
    props: ['routerName'],
    data () {
      return {
        dialog1: 'sandbox',
        dialog: false
      }
    },
    methods: {
      createRouter () {
        this.$router.push({name: 'sandbox', params: { routerName: this.routerName }})
      }
    }
  }
</script>