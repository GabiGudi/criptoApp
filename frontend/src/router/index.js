import { createRouter, createWebHistory } from 'vue-router'
import AltaCompraView       from '@/views/AltaCompraView.vue'
import HistorialView        from '@/views/HistorialView.vue'
import AltaClienteView      from '@/views/AltaClienteView.vue'
import VerTransaccionView   from '@/views/VerTransaccionView.vue'
import EditarTransaccionView from '@/views/EditarTransaccionView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        { path: '/',                         name: 'altaCompra',        component: AltaCompraView },
        { path: '/historial',                name: 'historial',         component: HistorialView },
        { path: '/clientes/nuevo',           name: 'altaCliente',       component: AltaClienteView },
        { path: '/transacciones/:id',        name: 'verTransaccion',    component: VerTransaccionView },
        { path: '/transacciones/:id/editar', name: 'editarTransaccion', component: EditarTransaccionView },
    ]
})

export default router