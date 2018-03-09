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
          v-form
            v-layout(column)
              v-flex
                v-text-field(
                  name="email"
                  label="Email"
                  id="email"
                  type="email"
                  required
                )
              v-flex
                v-text-field(
                  name="password"
                  label="Senha"
                  id="password"
                  type="password"
                  required
                )
              v-flex(
                class="text-xs-center"
                mt-5
              )
                v-btn(
                  primary
                  type="submit"
                ) Entrar
</template>

<script>
  export default {
    created () {
      this.$paper.navigation.setRightMenuVisible(false)
    }
  }
</script>
