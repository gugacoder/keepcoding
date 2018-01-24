<template lang="pug">
  v-flex(xs6)
    v-select(
      :label="field.title"
      :items="items"
      :name="field.name"
      item-value="text"
      v-model="field.value"
    )
</template>

<script>
  export default {
    props: ['field'],
    data: () => ({
      model: null,
      items: [
        { text: 'State 1' },
        { text: 'State 2' },
        { text: 'State 3' },
        { text: 'State 4' },
        { text: 'State 5' },
        { text: 'State 6' },
        { text: 'State 7' }
      ]
    })
  }
</script>
