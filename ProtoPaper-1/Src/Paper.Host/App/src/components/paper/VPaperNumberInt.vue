<template lang="pug">
  v-text-field(
    :label="field.title" 
    :value="field.value"
    :name="field.name"
    type="number"
  )
</template>

<script>
  export default {
    props: ['field']
  }
</script>
