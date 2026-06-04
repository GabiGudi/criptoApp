const URL_BASE = 'http://localhost:5003'

export default {
    // Obtener todas las transacciones
    async obtenerTransacciones() {
        const respuesta = await fetch(`${URL_BASE}/transacciones`)
        return respuesta.json()
    },

    // Crear una nueva transacción (compra o venta)
    async crearTransaccion(datos) {
        const respuesta = await fetch(`${URL_BASE}/transacciones`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(datos)
        })
        return respuesta.json()
    }
}