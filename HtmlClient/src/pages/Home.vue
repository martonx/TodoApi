<script setup>
import { onBeforeMount, reactive } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const baseUrl = 'http://localhost:5278';

const localState = reactive({
  toDos: [],
  showOnlyActive: true
});

onBeforeMount(async () => {
  await loadData();
});

async function filterIsReadyChanged()
{
  await loadData();
}

async function toDoIsReadyChanged(toDo)
{
  const response = await fetch(`${baseUrl}/update`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(toDo),
  });

  if (response.ok) {
    await loadData();
  }
}

async function editClicked(id)
{
  router.push(`/details/${id}`);
}

async function loadData() {
  let url = `${baseUrl}/list`;

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
      @change="filterIsReadyChanged"></input>
  </div>
  <ul>
    <li>
      <div>
        <span>ID</span>
        <span>Title</span>
        <span>Deadline</span>
        <span>IsReady</span>
        <span>Műveletek</span>
        <span>valami</span>
      </div>
    </li>
    <li v-for="toDo in localState.toDos">
      <div :class="{ red: toDo.expired }">
        <span>{{ toDo.id }}</span>
        <span>{{ toDo.title }}</span>
        <span>{{ toDo.deadline }}</span>
        <span>
          <input
            type="checkbox"
            v-model="toDo.isReady"
            @change="() => toDoIsReadyChanged(toDo)">
          </input>
        </span>
        <button @click="() => editClicked(toDo.id)">Edit</button>
        <button class="red">Delete</button>
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
