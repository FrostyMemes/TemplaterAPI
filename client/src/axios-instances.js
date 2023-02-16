import axios from "axios";

const api = axios.create({
    baseURL: `http://${process.env.REACT_APP_BACKEND_ADDRESS}`,
    url: '/api',
    method: 'get',
    responseType: 'text'
})

export default api