import api from "../http";

const serviceUrl = "markdown"

export function sendMarkdown(markdown) {
    return api.get(`${serviceUrl}`,{
        params: {
            markdown: markdown
        }
    })
}
