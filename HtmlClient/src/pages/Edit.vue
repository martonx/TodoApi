<script setup>
import { onBeforeMount, reactive } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const id = router.currentRoute.value.params.id;

onBeforeMount(async () => {
  //TODO:
});
</script>

<template>
  <h1>ToDo Details</h1>
</template>

<style scoped>

</style>
