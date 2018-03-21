<template lang="pug">
  v-flex(
    xs12
    right
    v-if="!$paper.state.selection()"
  )
    v-btn(
      color="primary"
      flat
      :icon="$breakpoint.xs"
      :disabled="!$paper.pagination.showFirst()"
      @click.stop="$paper.pagination.goToFirstPage()"
    )
      v-icon first_page
      span(class="hidden-xs-only") Primeira Página

    v-btn(
      color="primary"
      flat
      :icon="$breakpoint.xs"
      :disabled="!$paper.pagination.showPrevious()"
      @click.stop="$paper.pagination.goToPreviousPage()"
    )
      v-icon navigate_before
      span(class="hidden-xs-only") Anterior
      
    v-btn(
      color="primary"
      flat
      :icon="$breakpoint.xs"
      :disabled="!$paper.pagination.showNext()"
      @click.stop="$paper.pagination.goToNextPage()"
    )
      v-icon navigate_next
      span(class="hidden-xs-only") Próxima
</template>

<script>
  import Breakpoint from '../mixins/Breakpoint.js'
  export default {
    mixins: [
      Breakpoint
    ]
  }
</script>
