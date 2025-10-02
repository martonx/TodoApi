import { createApp } from 'vue'
import { createWebHistory , createRouter } from 'vue-router'
import './style.css'
import App from './App.vue'
import HomeView from './pages/Home.vue'
import EditView from './pages/Edit.vue'
import NotFoundView from './pages/NotFound.vue'

const routes = [
  { path: '/', component: HomeView },
  { path: '/details/:id', component: EditView },
  { path: '/:pathMatch(.*)*', component: NotFoundView },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});

createApp(App)
.use(router)
.mount('#app');
