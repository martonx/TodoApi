<script setup>
import { onBeforeMount, reactive } from 'vue'

const localState = reactive({
  toDos: []
});

onBeforeMount(async () => {
  var response = await fetch('http://localhost:5278/list?isReady=false');
  var result = await response.json();
  result.forEach(toDo => localState.toDos.push(toDo));
});
</script>

<template>
  <h1>Hello from Home page</h1>
  <h2>Todos</h2>
  <ul>
    <li v-for="toDo in localState.toDos">
      <span>{{ toDo.id }}</span>
      <span>{{ toDo.title }}</span>
    </li>
  </ul>
</template>

<style scoped>

</style>
