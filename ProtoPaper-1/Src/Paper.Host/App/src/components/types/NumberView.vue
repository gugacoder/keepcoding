
<template>
  <v-text-field 
    :label="field.title" 
    :value="field.value"
    :name="field.name"
    v-model="field.value"
    type="number">
  </v-text-field>
</template>

<script>
  export default {
    data: () => ({
      number: ''
    }),
    props: ['field']
  }
</script>
