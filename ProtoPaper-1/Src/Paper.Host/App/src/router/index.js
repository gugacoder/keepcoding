import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/components/HomeView'
import NotFound from '@/components/NotFoundView'
import Sandbox from '@/components/SandboxView'
import Page from '@/components/PageView'

Vue.use(Router)

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home
    },
    {
      path: '/page/:path(.*)*',
      name: 'page',
      component: Page
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
