import { createRouter, createWebHistory } from 'vue-router'
import AltaCompraView from '@/views/AltaCompraView.vue'
import HistorialView from '@/views/HistorialView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'altaCompra',
            component: AltaCompraView
        },
        {
            path: '/historial',
            name: 'historial',
            component: HistorialView
        }
    ]
})

export default router