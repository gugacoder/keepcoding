<template lang="pug">
  v-text-field(
    :label="field.title" 
    :value="field.value"
    :name="field.name"
    v-model="maskNumber"
    v-mask-number
  )
</template>

<script>
  export default {
    data: () => ({
      number: ''
    }),
    props: ['field']
  }
</script>
