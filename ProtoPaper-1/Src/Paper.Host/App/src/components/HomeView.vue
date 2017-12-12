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
    data () {
      return {
        max25chars: (v) => v.length <= 25 || 'Input too long!',
        tmp: '',
        search: '',
        pagination: {},
        headers: [
          {
            text: 'Dessert (100g serving)',
            align: 'left',
            sortable: false,
            value: 'name'
          },
          { text: 'Calories', value: 'calories' },
          { text: 'Fat (g)', value: 'fat' },
          { text: 'Carbs (g)', value: 'carbs' },
          { text: 'Protein (g)', value: 'protein' },
          { text: 'Sodium (mg)', value: 'sodium' },
          { text: 'Calcium (%)', value: 'calcium' },
          { text: 'Iron (%)', value: 'iron' }
        ],
        items: [
          {
            'class': 'order list-item',
            'rel': 'order',
            'properties': {
              'value': false,
              'name': 'Frozen Yogurt',
              'calories': 159,
              'fat': 6.0,
              'carbs': 24,
              'protein': 4.0,
              'sodium': 87,
              'calcium': '14%',
              'iron': '1%'
            }
          },
          {
            'class': 'order list-item',
            'rel': 'order',
            'properties': {
              'value': false,
              'name': 'Ice cream sandwich',
              'calories': 237,
              'fat': 9.0,
              'carbs': 37,
              'protein': 4.3,
              'sodium': 129,
              'calcium': '8%',
              'iron': '1%'
            }
          }
        ]
      }
    }
  }
</script>