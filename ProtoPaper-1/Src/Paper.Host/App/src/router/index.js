import Vue from 'vue'
import Router from 'vue-router'
import NotFound from '@/components/NotFoundPage'
import ErrorPage from '@/components/ErrorPage'
import Sandbox from '@/components/Sandbox'
import Page from '@/components/PageView'
import Home from '@/components/Home'

Vue.use(Router)

export default new Router({
  root: '/',
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
    },
    {
      path: '/error',
      name: 'error',
      component: ErrorPage,
      props: true
    }
  ]
})
