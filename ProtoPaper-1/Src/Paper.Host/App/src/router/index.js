import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/components/Home'
import NotFound from '@/components/NotFoundPage'
import ErrorPage from '@/components/ErrorPage'
import Sandbox from '@/components/Sandbox'
import Page from '@/components/PageView'

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
