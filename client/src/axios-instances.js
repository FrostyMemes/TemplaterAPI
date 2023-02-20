import axios from "axios";

const apiUrl = `http://${process.env.REACT_APP_BACKEND_ADDRESS}/api`

const api = axios.create({
    baseURL: apiUrl
})


export default api