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
  result.forEach(toDo => {
    const deadline = new Date(toDo.deadline);
    toDo.expired = !toDo.isReady && deadline < new Date();
    localState.toDos.push(toDo);
  });
}
</script>

<template>
  <h1>Todos</h1>
  <div>
    <label>Show only active:</label>
    <input
      type="checkbox"
      v-model="localState.showOnlyActive"
      @change="isReadyChanged"></input>
  </div>
  <ul>
    <li>
      <div>
        <span>ID</span>
        <span>Title</span>
        <span>Deadline</span>
        <span>IsReady</span>
      </div>
    </li>
    <li v-for="toDo in localState.toDos">
      <div :class="{ red: toDo.expired }">
        <span>{{ toDo.id }}</span>
        <span>{{ toDo.title }}</span>
        <span>{{ toDo.deadline }}</span>
        <span>{{ toDo.isReady }}</span>
      </div>
    </li>
  </ul>
</template>

<style scoped>
li div {
  display: flex;
  justify-content: space-between;
}
.red {
  background-color: red;
}
</style>
