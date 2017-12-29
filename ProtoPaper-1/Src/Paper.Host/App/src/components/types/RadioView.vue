<template>
  <v-radio-group v-model="field.value" :mandatory="false">
    <v-radio :label="field.title" :value="field.value"></v-radio>
  </v-radio-group>
</template>  

<script>
  export default {
    props: ['field']
  }
</script>
