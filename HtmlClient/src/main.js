import { createApp } from 'vue'
import { createWebHistory , createRouter } from 'vue-router'
import './style.css'
import App from './App.vue'
import HomeView from './pages/Home.vue'
import AboutView from './pages/About.vue'

const routes = [
  { path: '/', component: HomeView },
  { path: '/about', component: AboutView },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});

createApp(App)
.use(router)
.mount('#app');
