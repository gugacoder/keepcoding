<template>
  <v-card>
    <v-card-title>
      Nutrition
      <v-spacer></v-spacer>
      <v-text-field
        append-icon="search"
        label="Buscar"
        single-line
        hide-details
        v-model="search">
      </v-text-field>
    </v-card-title>
    <v-data-table
        v-bind:headers="headers"
        v-bind:items="items"
        v-bind:search="search"
        rows-per-page-text="Itens por Página">
      <template slot="items" slot-scope="items">
        <td>
          <v-edit-dialog lazy> {{ items.item.properties.name }}
            <v-text-field
              slot="input"
              label="Edit"
              v-model="items.item.properties.name"
              single-line
              counter
              :rules="[max25chars]">
            </v-text-field>
          </v-edit-dialog>
        </td>
        <td class="text-xs-right">{{ items.item.properties.calories }}</td>
        <td class="text-xs-right">{{ items.item.properties.fat }}</td>
        <td class="text-xs-right">{{ items.item.properties.carbs }}</td>
        <td class="text-xs-right">{{ items.item.properties.protein }}</td>
        <td class="text-xs-right">{{ items.item.properties.sodium }}</td>
        <td class="text-xs-right">{{ items.item.properties.calcium }}</td>
        <td class="text-xs-right">
          <v-edit-dialog
            @open="tmp = items.item.properties.iron"
            @save="items.item.properties.iron = tmp || items.item.properties.iron"
            large
            lazy>
            <div>{{ items.item.properties.iron }}</div>
            <div slot="input" class="mt-3 title">Update Iron</div>
            <v-text-field
              slot="input"
              label="Edit"
              v-model="tmp"
              single-line
              counter
              autofocus
              :rules="[max25chars]">
            </v-text-field>
          </v-edit-dialog>
        </td>
      </template>
      <template slot="pageText" slot-scope="{ pageStart, pageStop }">
        Página {{ pageStart }} de {{ pageStop }}
      </template>
    </v-data-table>
  </v-card>
</template>

<script>
  export default {
    props: ['siren'],
    data () {
      return {
        max25chars: (v) => v.length <= 25 || 'Input too long!',
        tmp: '',
        search: '',
        pagination: {},
        headers: this.siren.properties.headers,
        items: this.siren.entities,
        buttons: this.siren.actions
      }
    },
    beforeRouteUpdate (to, from, next) {
      next()
    }
  }
</script>