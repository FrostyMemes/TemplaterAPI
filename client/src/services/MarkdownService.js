import api from "../http";

const serviceUrl = "markdown"

export function SendMarkdown(markdown) {
    return api.get(`${serviceUrl}`,{
        params: {
            markdown: markdown
        }
    })
}
