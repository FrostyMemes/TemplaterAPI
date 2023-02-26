import React from 'react';
import {useEffect, useState} from "react";
import useDebounce from "../hooks/use-debounce";
import api from "../../axios-instances";
import TemplateItem from "../TemplateItem";


const EditorMarkdown = (props) => {

    const [templates, setTemplates] = useState([])
    const [mdMarkdown, setMarkdown] = useState('')
    const [mdRender, setRender] = useState('')
    const [mdTitle, setTitle] = useState('')

    const debouncedSearchTerm = useDebounce(mdMarkdown, 700);

    function renderTemplate(value) {
        api.get(process.env.REACT_APP_BACKEND_MARKDOWN_ADDRESS,{
            params:{
                markdown: mdMarkdown
            }
        })
            .then(response => setRender(response.data))
            .catch(error => setRender(error.message))
    }

    function saveTemplate() {
        api.post(`${process.env.REACT_APP_BACKEND_TEMPLATE_ADDRESS}`,
            {
                title: mdTitle,
                markdown: mdMarkdown,
                markup: mdRender
            })
            .then(response => console.log(response.data))
            .catch(error => console.log(error.message))
    }

    useEffect(() =>{
            api.get(`${process.env.REACT_APP_BACKEND_TEMPLATE_ADDRESS}/getall`)
                .then(response => setTemplates(response.data))
                .catch(error => console.log(error.message))
        }, []
    )

    useEffect(
        () => {
            if (debouncedSearchTerm) {
                renderTemplate(debouncedSearchTerm)
            } else {
                setRender('');
            }
        },
        [debouncedSearchTerm]
    );

    return (
        <div className="editor">
            <h1>{mdTitle}</h1>
            <input onChange={(e) => setTitle(e.target.value)}/>
            <div className="editor-area">
                <div className="input-area">
                    <textarea placeholder={process.env.REACT_APP_BACKEND_ADDRESS}
                              onChange={(e) => setMarkdown(e.target.value)}>
                    </textarea>
                </div>
                <div className="render-area" dangerouslySetInnerHTML={{__html: mdRender}}></div>
            </div>
            <input type="button" onClick={saveTemplate} value="Save to DataBase"/>
            <div className="template__list">
                {templates.map(template =>
                    <TemplateItem template={template} key={template.id}/>
                )}
            </div>
        </div>
    );
};

export default EditorMarkdown;