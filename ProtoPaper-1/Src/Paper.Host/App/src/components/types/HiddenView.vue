
<template>
  <v-text-field 
    :label="field.name" 
    :value="field.value"
    v-model="field.value"
    hidden>
  </v-text-field>
</template>

<script>
  export default {
    props: ['field']
  }
</script>
