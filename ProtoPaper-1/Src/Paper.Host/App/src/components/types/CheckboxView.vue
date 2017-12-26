<template>
  <v-checkbox
    color="green"
    v-model="field.value">
    <div slot="label" @click.stop="">
      {{field.name}}
    </div>
  </v-checkbox>
</template>

<script>
  export default {
    props: ['field']
  }
</script>
