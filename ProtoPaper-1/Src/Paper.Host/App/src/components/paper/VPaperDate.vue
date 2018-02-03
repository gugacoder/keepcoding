<template lang="pug">
  v-layout(
    row 
    wrap
  )
    v-flex(
      xs11 
      sm5
    )
      v-menu(
        lazy
        :close-on-content-click="false"
        v-model="menu"
        transition="scale-transition"
        offset-y
        full-width
        :nudge-right="40"
        max-width="290px"
        min-width="290px"
      )
        v-text-field(
          :name="field.name"
          :label="field.title"
          :value="field.value"
          slot="activator"
          readonly
          v-model="dateFormatted"
          prepend-icon="event"
        )
        v-date-picker(
          v-model="date" 
          :first-day-of-week="1"
          locale="pt-br"
          @input="dateFormatted = formatDate($event)" 
          no-title 
          scrollable 
          actions
        )
          template(slot-scope="{ save, cancel }")
            v-card-actions
              v-spacer
              v-btn(
                flat 
                color="primary" 
                @click="cancel"
              ) Cancelar

              v-btn(
                flat 
                color="primary" 
                @click="save"
              ) OK
</template>

<script>
  export default {
    props: ['field'],
    data: () => ({
      date: null,
      dateFormatted: null,
      menu: false
    }),
    methods: {
      formatDate (date) {
        if (!date) {
          return null
        }

        const [year, month, day] = date.split('-')
        return `${day}/${month}/${year}`
      },
      parseDate (date) {
        if (!date) {
          return null
        }

        const [month, day, year] = date.split('/')
        return `${day}-${month}-${year}`
      }
    }
  }
</script>
