const URL_BASE = 'http://localhost:5003'

export default {
    async obtenerClientes() {
        const respuesta = await fetch(`${URL_BASE}/clientes`)
        return respuesta.json()
    },

    async crearCliente(datos) {
        const respuesta = await fetch(`${URL_BASE}/clientes`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(datos)
        })
        return respuesta.json()
    },

    async obtenerTransacciones() {
        const respuesta = await fetch(`${URL_BASE}/transacciones`)
        return respuesta.json()
    },

    async obtenerTransaccionPorId(id) {
        const respuesta = await fetch(`${URL_BASE}/transacciones/${id}`)
        return respuesta.json()
    },

    async crearTransaccion(datos) {
        const respuesta = await fetch(`${URL_BASE}/transacciones`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(datos)
        })
        return respuesta.json()
    },

    async actualizarTransaccion(id, datos) {
        const respuesta = await fetch(`${URL_BASE}/transacciones/${id}`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(datos)
        })
        return respuesta.json()
    },

    async eliminarTransaccion(id) {
        const respuesta = await fetch(`${URL_BASE}/transacciones/${id}`, {
            method: 'DELETE'
        })
        return respuesta.ok
    }
}