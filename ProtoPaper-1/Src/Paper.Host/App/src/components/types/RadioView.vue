<template>
  <v-radio-group v-model="field" :mandatory="false">
    <v-radio :label="field.name" :value="field.value"></v-radio>
  </v-radio-group>
</template>  

<script>
  export default {
    props: ['field']
  }
</script>
