<template>
  <v-dialog v-model="dialog" max-width="500px">
    <v-card>
      <v-card-title>
        Dialog 2
      </v-card-title>
      <v-card-text>
        <v-select
          :items="select"
          label="A Select List"
          item-value="text"
        ></v-select>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" flat @click.stop="close()">Close</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    props: {
      'showDialog': Boolean
    },
    data: () => ({
      dialog: this.showDialog,
      items: [
        {
          title: 'Click Me'
        },
        {
          title: 'Click Me'
        },
        {
          title: 'Click Me'
        },
        {
          title: 'Click Me 2'
        }
      ],
      select: [
        { text: 'State 1' },
        { text: 'State 2' },
        { text: 'State 3' },
        { text: 'State 4' },
        { text: 'State 5' },
        { text: 'State 6' },
        { text: 'State 7' }
      ]
    }),
    methods: {
      close () {
        this.dialog = false
      }
    }
  }
</script>
