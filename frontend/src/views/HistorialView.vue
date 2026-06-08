<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/api.js'

const router = useRouter()

let transacciones = ref([])
let clientes      = ref([])
let cliente_filtro = ref('')
let cargando      = ref(true)
let modal         = ref(null) // guarda la transacción a borrar

const infoCripto = {
    bitcoin: { simbolo: '₿', color: '#f7931a', fondo: '#2a1500' },
    eth:     { simbolo: 'Ξ', color: '#627eea', fondo: '#0f1135' },
    usdc:    { simbolo: '$', color: '#2775ca', fondo: '#001428' },
    sol:     { simbolo: '◎', color: '#9945ff', fondo: '#150a28' },
    dai:     { simbolo: '◈', color: '#f5ac37', fondo: '#2a1800' },
}

function obtenerCripto(code) {
    return infoCripto[code] || { simbolo: '?', color: '#8b949e', fondo: '#21262d' }
}

async function cargarDatos() {
    cargando.value = true
    try {
        const [txs, cls] = await Promise.all([
            api.obtenerTransacciones(),
            api.obtenerClientes()
        ])
        transacciones.value = txs
        clientes.value = cls
    } finally {
        cargando.value = false
    }
}

// Transacciones filtradas por cliente
const transaccionesFiltradas = () => {
    if (!cliente_filtro.value) return transacciones.value
    return transacciones.value.filter(
        t => t.cliente_id === Number(cliente_filtro.value)
    )
}

function nombreCliente(id) {
    if (!id) return '—'
    const c = clientes.value.find(c => c.id === id)
    return c ? c.nombre : '—'
}

function confirmarBorrar(transaccion) {
    modal.value = transaccion
}

async function borrar() {
    if (!modal.value) return
    await api.eliminarTransaccion(modal.value.id)
    modal.value = null
    await cargarDatos()
}

onMounted(cargarDatos)
</script>

<template>
    <div class="pagina">
        <div class="header">
            <div>
                <h1>Historial de movimientos</h1>
                <p class="subtitulo" v-if="!cargando">
                    {{ transaccionesFiltradas().length }} operaciones
                </p>
            </div>
            <button class="btn-actualizar" @click="cargarDatos">↻ Actualizar</button>
        </div>

        <!-- Filtro por cliente -->
        <div class="filtro" v-if="clientes.length > 0">
            <select v-model="cliente_filtro">
                <option value="">Todos los clientes</option>
                <option v-for="c in clientes" :key="c.id" :value="c.id">
                    {{ c.nombre }}
                </option>
            </select>
        </div>

        <p class="cargando" v-if="cargando">Cargando historial...</p>
        <p class="vacio" v-else-if="transaccionesFiltradas().length === 0">
            No hay operaciones registradas.
        </p>

        <div class="tabla-wrapper" v-else>
            <table>
                <thead>
                    <tr>
                        <th>Fecha</th>
                        <th>Activo</th>
                        <th>Tipo</th>
                        <th>Cliente</th>
                        <th class="derecha">Cantidad</th>
                        <th class="derecha">Total ARS</th>
                        <th class="centro">Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="t in transaccionesFiltradas()" :key="t.id">
                        <td class="fecha">
                            {{ new Date(t.datetime).toLocaleDateString('es-AR') }}
                            <span class="hora">
                                {{ new Date(t.datetime).toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' }) }}
                            </span>
                        </td>
                        <td>
                            <div class="cripto-badge"
                                :style="{ background: obtenerCripto(t.crypto_code).fondo }">
                                <span :style="{ color: obtenerCripto(t.crypto_code).color }">
                                    {{ obtenerCripto(t.crypto_code).simbolo }}
                                </span>
                                <span class="cripto-code"
                                    :style="{ color: obtenerCripto(t.crypto_code).color }">
                                    {{ t.crypto_code.toUpperCase() }}
                                </span>
                            </div>
                        </td>
                        <td>
                            <span class="tipo-badge"
                                :class="t.action === 'purchase' ? 'compra' : 'venta'">
                                {{ t.action === 'purchase' ? '▲ Compra' : '▼ Venta' }}
                            </span>
                        </td>
                        <td class="cliente">{{ nombreCliente(t.cliente_id) }}</td>
                        <td class="derecha mono">
                            <span :class="t.action === 'purchase' ? 'pos' : 'neg'">
                                {{ t.action === 'purchase' ? '+' : '-' }}{{ t.crypto_amount }}
                            </span>
                        </td>
                        <td class="derecha mono monto">
                            $ {{ t.money.toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) }}
                        </td>
                        <td class="centro">
                            <div class="acciones">
                                <button class="btn-ver"
                                    @click="router.push(`/transacciones/${t.id}`)">
                                    Ver
                                </button>
                                <button class="btn-editar"
                                    @click="router.push(`/transacciones/${t.id}/editar`)">
                                    Editar
                                </button>
                                <button class="btn-borrar"
                                    @click="confirmarBorrar(t)">
                                    Borrar
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Modal de confirmación de borrado -->
        <div class="modal-overlay" v-if="modal" @click.self="modal = null">
            <div class="modal">
                <h3>¿Confirmar borrado?</h3>
                <p>
                    Vas a eliminar la transacción de
                    <strong>{{ modal.crypto_code.toUpperCase() }}</strong>
                    por <strong>$ {{ modal.money.toLocaleString('es-AR') }}</strong>.
                    Esta acción no se puede deshacer.
                </p>
                <div class="modal-acciones">
                    <button class="btn-cancelar" @click="modal = null">Cancelar</button>
                    <button class="btn-confirmar" @click="borrar">Sí, borrar</button>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.pagina { padding-top: 0.5rem; }

.header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 1rem;
}

h1 {
    font-size: 1.4rem;
    font-weight: 600;
    color: #e6edf3;
    margin-bottom: 4px;
}

.subtitulo { color: #8b949e; font-size: 0.85rem; }

.btn-actualizar {
    background: #21262d;
    border: 1px solid #30363d;
    color: #8b949e;
    border-radius: 6px;
    padding: 0.4rem 0.8rem;
    font-size: 0.85rem;
    cursor: pointer;
    transition: all 0.15s;
}
.btn-actualizar:hover { color: #e6edf3; border-color: #8b949e; }

.filtro { margin-bottom: 1rem; }
.filtro select {
    background: #21262d;
    border: 1px solid #30363d;
    border-radius: 8px;
    padding: 0.5rem 0.8rem;
    color: #e6edf3;
    font-size: 0.9rem;
    cursor: pointer;
    min-width: 220px;
}

.cargando, .vacio {
    color: #8b949e;
    text-align: center;
    padding: 3rem;
}

.tabla-wrapper {
    background: #161b22;
    border: 1px solid #30363d;
    border-radius: 12px;
    overflow: hidden;
}

table { width: 100%; border-collapse: collapse; }

thead { background: #21262d; }

th {
    padding: 0.75rem 1rem;
    font-size: 0.75rem;
    font-weight: 500;
    color: #8b949e;
    text-transform: uppercase;
    letter-spacing: 0.05em;
    text-align: left;
    border-bottom: 1px solid #30363d;
}

td {
    padding: 0.85rem 1rem;
    border-bottom: 1px solid #21262d;
    font-size: 0.9rem;
    color: #e6edf3;
}

tr:last-child td { border-bottom: none; }
tr:hover td { background: #1c2128; }

.fecha { color: #e6edf3; white-space: nowrap; }
.hora { display: block; font-size: 0.75rem; color: #8b949e; }

.cripto-badge {
    display: inline-flex;
    align-items: center;
    gap: 5px;
    padding: 3px 8px;
    border-radius: 6px;
}
.cripto-code { font-size: 0.8rem; font-weight: 600; }

.tipo-badge {
    font-size: 0.78rem;
    font-weight: 500;
    padding: 3px 8px;
    border-radius: 12px;
}
.compra { background: #0d2b1f; color: #3fb950; }
.venta  { background: #2d1117; color: #f85149; }

.cliente { color: #8b949e; font-size: 0.85rem; }

.mono { font-family: 'SF Mono', 'Consolas', monospace; font-size: 0.85rem; }
.derecha { text-align: right; }
.centro  { text-align: center; }
.pos { color: #3fb950; }
.neg { color: #f85149; }
.monto { color: #e6edf3; font-weight: 500; }

/* Botones de acciones */
.acciones { display: flex; gap: 6px; justify-content: center; }

.btn-ver, .btn-editar, .btn-borrar {
    padding: 4px 10px;
    border-radius: 6px;
    font-size: 0.78rem;
    cursor: pointer;
    border: 1px solid;
    transition: all 0.15s;
}

.btn-ver {
    background: transparent;
    border-color: #30363d;
    color: #8b949e;
}
.btn-ver:hover { border-color: #58a6ff; color: #58a6ff; }

.btn-editar {
    background: transparent;
    border-color: #30363d;
    color: #8b949e;
}
.btn-editar:hover { border-color: #f0b429; color: #f0b429; }

.btn-borrar {
    background: transparent;
    border-color: #30363d;
    color: #8b949e;
}
.btn-borrar:hover { border-color: #f85149; color: #f85149; }

/* Modal */
.modal-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0,0,0,0.7);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 100;
}

.modal {
    background: #161b22;
    border: 1px solid #30363d;
    border-radius: 12px;
    padding: 1.5rem;
    max-width: 420px;
    width: 90%;
}

.modal h3 {
    color: #e6edf3;
    font-size: 1.1rem;
    margin-bottom: 0.75rem;
}

.modal p {
    color: #8b949e;
    font-size: 0.9rem;
    line-height: 1.6;
    margin-bottom: 1.2rem;
}

.modal p strong { color: #e6edf3; }

.modal-acciones { display: flex; gap: 8px; justify-content: flex-end; }

.btn-cancelar {
    background: #21262d;
    border: 1px solid #30363d;
    color: #8b949e;
    padding: 0.5rem 1rem;
    border-radius: 6px;
    cursor: pointer;
    font-size: 0.9rem;
}
.btn-cancelar:hover { color: #e6edf3; }

.btn-confirmar {
    background: #da3633;
    border: none;
    color: #fff;
    padding: 0.5rem 1rem;
    border-radius: 6px;
    cursor: pointer;
    font-size: 0.9rem;
    font-weight: 500;
}
.btn-confirmar:hover { background: #f85149; }
</style>