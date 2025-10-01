<script setup>
import { onBeforeMount, reactive } from 'vue'

const localState = reactive({
  toDos: [],
  showOnlyActive: true
});

onBeforeMount(async () => {
  await loadData();
});

async function isReadyChanged()
{
  await loadData();
}

async function loadData() {
  let url = 'http://localhost:5278/list';

  if (localState.showOnlyActive){
    url += '?isReady=false'
  }

  const response = await fetch(url);
  const result = await response.json();
  localState.toDos = [];
  result.forEach(toDo => localState.toDos.push(toDo));
}
</script>

<template>
  <h1>Hello from Home page</h1>
  <h2>Todos</h2>
  <div>
    <label>Show only active:</label>
    <input
      type="checkbox"
      v-model="localState.showOnlyActive"
      @change="isReadyChanged"></input>
  </div>
  <ul>
    <li v-for="toDo in localState.toDos">
      <span>{{ toDo.id }}</span>
      <span>{{ toDo.title }}</span>
      <span>{{ toDo.deadline }}</span>
      <span>{{ toDo.isReady }}</span>
    </li>
  </ul>
</template>

<style scoped>

</style>
