<template>
  <v-checkbox
    color="green"
    :name="field.name"
    :value="field.value"
    v-model="field.value"
    @click.stop="field.value = !field.value">
    <div slot="label">
      {{field.title}}
    </div>
  </v-checkbox>
</template>

<script>
  export default {
    props: ['field']
  }
</script>
