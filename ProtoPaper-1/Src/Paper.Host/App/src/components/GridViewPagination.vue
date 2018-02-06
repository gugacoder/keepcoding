<template lang="pug">
  v-flex(
    xs12
    right
    v-if="!$store.state.selection.selectionState"
  )
    v-btn(
      color="primary"
      flat
      :disabled="!$paper.pagination.showFirst()"
      @click.stop="$paper.pagination.goToFirstPage()"
    )
      v-icon first_page
      span Primeira Página

    v-btn(
      color="primary"
      flat
      :disabled="!$paper.pagination.showPrevious()"
      @click.stop="$paper.pagination.goToPreviousPage()"
    )
      v-icon navigate_before
      span Anterior
      
    v-btn(
      color="primary"
      flat
      :disabled="!$paper.pagination.showNext()"
      @click.stop="$paper.pagination.goToNextPage()"
    )
      v-icon navigate_next
      span Próxima
</template>
