<template lang="pug">
  v-container(
    fluid
  )
    v-flex(
      xs10
      sm6
      offset-sm3
    )

      v-card
        v-card-title(primary-title)
          h2 Login

        v-card-text
          v-form(v-on:submit="login()")
            v-layout(column)

              v-flex
                v-text-field(
                  name="email"
                  label="Email"
                  id="email"
                  type="email"
                  v-model="data.body.username"
                  required
                )

              v-flex
                v-text-field(
                  name="password"
                  label="Senha"
                  id="password"
                  type="password"
                  v-model="data.body.password"
                  required
                )

              v-flex
                v-switch(
                  v-model="data.rememberMe"
                  label="Lembrar-me"
                )

              v-flex(
                class="text-xs-center"
                mt-5
              )
                v-btn(
                  color="primary"
                  type="submit"
                ) Entrar

                v-btn(
                  color="primary"
                  @click.stop="signup()"
                ) Quero me cadastrar

              div(
                v-show="error"
                style="color:red; word-wrap:break-word;"
              ) {{ error | json }}
</template>

<script>
  export default {
    data: () => ({
      data: {
        body: {
          username: 'francinescosta@gmail.com',
          password: '#Fr4nc1n3',
          client_id: 'AT1gdHZ40YSs59xR2COd67Z1DbRrIviB',
          response_type: 'token',
          connection: 'Username-Password-Authentication',
          redirect_uri: 'http://localhost:5000/home'
        },
        rememberMe: false
      },
      error: null
    }),

    created () {
      this.$paper.navigation.setRightMenuVisible(false)
    },

    methods: {
      login () {
        console.log('login', this.$auth)
        this.$auth.login({
          data: this.data.body,
          success: function () {
            console.log('Usuário logado com sucesso.')
          },
          error: function () {
            console.log('Usuário e/ou senha inválidos.')
          },
          rememberMe: this.data.rememberMe,
          redirect: { name: 'about' }
        })
        // // this.$paper.authentication.login(this.username, this.password)
      },

      signup () {
        console.log('Signup')
      },

      logout () {
        console.log('Logout')
      }
    }
  }
</script>
