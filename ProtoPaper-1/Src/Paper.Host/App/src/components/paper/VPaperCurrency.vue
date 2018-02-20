<template lang="pug">
  v-text-field(
    :label="field.title" 
    :value="formatValue"
    :name="field.name"
    v-model="field.value"
    v-money="money"
  )
</template>

<script>
  import {VMoney} from 'v-money'
  export default {
    props: ['field'],
    data: () => ({
      money: {
        decimal: ',',
        thousands: '.',
        prefix: 'R$ ',
        precision: 2,
        masked: false
      }
    }),
    directives: {
      money: VMoney
    },
    computed: {
      formatValue () {
        var value = this.field.value
        if (value) {
          value = value.replace(/R\$/g, '').trim()
        }
        return value
      }
    }
  }
</script>
