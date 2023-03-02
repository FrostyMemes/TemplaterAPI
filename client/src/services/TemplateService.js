import api from "../http";

const serviceUrl = "template"

export function getAllTemplates() {
    return api.get(`${serviceUrl}/getall`)
}

export function getTemplateById(id) {
    return api.get(`${serviceUrl}/${id}`)
}

export function saveTemplate(template) {
    return api.post(serviceUrl, template)
}

export function DeleteTemplate(id) {
    return api.delete(`${serviceUrl}/${id}`)
}




