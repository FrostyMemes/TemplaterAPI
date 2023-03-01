import React from 'react';
import api from "../../axios-instances";
import {useEffect, useState} from "react";
import useDebounce from "../hooks/use-debounce";
import EditorMarkdownArea from "./EditorMarkdownArea";
import TemplateList from "../TemplateList";

const Editor = (props) => {

    const [templates, setTemplates] = useState([])
    const [mdMarkdown, setMarkdown] = useState('')
    const [mdRender, setRender] = useState('')
    const [mdTitle, setTitle] = useState('')

    const debouncedSearchTerm = useDebounce(mdMarkdown, 700);
    const renderTemplate = (markdown) => {
        api.get(process.env.REACT_APP_BACKEND_MARKDOWN_ADDRESS, {
            params: {
                markdown: markdown
            }
        })
            .then(response => setRender(response.data))
            .catch(error => setRender(error.message))
    }
    const saveTemplate = () => {
        api.post(`${process.env.REACT_APP_BACKEND_TEMPLATE_ADDRESS}`,
            {
                title: mdTitle,
                markdown: mdMarkdown,
                markup: mdRender
            })
            .then(response => {
                console.log(response.data)
                setTemplates([...templates, response.data])
            })
            .catch(error => console.log(error.message))
    }

    useEffect(() => {
            api.get(`${process.env.REACT_APP_BACKEND_TEMPLATE_ADDRESS}/getall`)
                .then(response => {setTemplates(response.data)})
                .catch(error => console.log(error.message))
        }, [])
    useEffect(() => {
            if (debouncedSearchTerm) {
                renderTemplate(debouncedSearchTerm)
            } else {
                setRender('');
            }
        }, [debouncedSearchTerm])

    return (
        <div className="editor">
            <h1>{mdTitle}</h1>
            <input onChange={(e) => setTitle(e.target.value)}/>
            <EditorMarkdownArea
                onChange={markdown => setMarkdown(markdown)}
                render={mdRender}/>
            <input type="button" onClick={saveTemplate} value="Save"/>
            <TemplateList templateList={templates}/>
        </div>
    );
};

export default Editor;