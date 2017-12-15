import Vue from 'vue'
import Router from 'vue-router'
import Grid from '@/components/GridView'
import Home from '@/components/HomeView'
import NotFound from '@/components/NotFoundView'
import Sandbox from '@/components/SandboxView'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home
    },
    {
      path: '/page/:path(.*)*',
      name: 'page',
      component: Grid,
      props: true
    },
    {
      path: '/notFound',
      name: 'notFound',
      component: NotFound,
      props: true
    },
    {
      path: '/sandbox',
      name: 'sandbox',
      component: Sandbox,
      props: true
    }
  ]
})
