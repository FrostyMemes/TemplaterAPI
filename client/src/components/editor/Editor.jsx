import React from 'react';
import api from "../../http";
import {useEffect, useState} from "react";
import useDebounce from "../hooks/use-debounce";
import EditorMarkdownArea from "./EditorMarkdownArea";
import TemplateList from "../TemplateList";
import * as TemplateService from "../../services/TemplateService"

const Editor = (props) => {

    const [templates, setTemplates] = useState([])
    const [mdMarkdown, setMarkdown] = useState('')
    const [mdRender, setRender] = useState('')
    const [mdTitle, setTitle] = useState('')

    const debouncedSearchTerm = useDebounce(mdMarkdown, 700);

    const renderTemplate = (markdown) => {
        api.get('markdown', {
            params: {
                markdown: markdown
            }
        })
            .then(response => setRender(response.data))
            .catch(error => setRender(error.message))
    }

    const saveTemplate = () => {
        TemplateService.saveTemplate({
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
        TemplateService.getAllTemplates()
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