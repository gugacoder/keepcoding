<template lang="pug">
  v-container(
    fluid
    fill-height
  )
    v-container(
      fluid 
      class="text-xs-center"
    )
      v-layout(
        flex 
        wrap
        row
        justify-space-between
      )
        v-flex(xs12)
          h1 404

        v-flex(xs12)
          h2 Página não encontrada!
          
        v-flex(
          xs12 
          class="mt-5"
        )
          v-dialog(
            v-model="dialog" 
            max-width="300px"
          )
            v-btn(
              color="primary" 
              dark 
              slot="activator"
            )
              | Projetar essa rota
            
            v-card
              v-card-title 
                | Selecione um módulo

              v-divider

              v-card-text
                v-radio-group(
                  v-model="dialog1"
                  column
                )
                  v-radio(
                    label="Sandbox" 
                    value="sandbox"
                  )

              v-divider

              v-card-actions
                v-btn(
                  color="primary" 
                  flat 
                  @click.native="dialog=false"
                ) Cancelar

                v-btn(
                  color="primary" 
                  flat 
                  @click.native="createRouter"
                ) Ok
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