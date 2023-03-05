import React from 'react';
import api from "../../../http";
import {useEffect, useState} from "react";
import {useSelector, useDispatch} from "react-redux";
import {addTemplate, getAllTemplates, removeTemplate} from "../../../store/templateSlice";
import useDebounce from "../../hooks/use-debounce";
import EditorMarkdownArea from "./EditorMarkdownArea";
import TemplateList from "../TemplateList/TemplateList";
import {SendMarkdown} from "../../../services/MarkdownService";
import * as TemplateService from "../../../services/TemplateService"

const Editor = () => {

    const templates = useSelector(state => state.templates.templates)
    const dispatch = useDispatch()

    const [templatess, setTemplates] = useState([])
    const [mdMarkdown, setMarkdown] = useState('')
    const [mdRender, setRender] = useState('')
    const [mdTitle, setTitle] = useState('')

    const debouncedSearchTerm = useDebounce(mdMarkdown, 700);

    const renderTemplate = (markdown) => {
        SendMarkdown(markdown)
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
            dispatch(addTemplate(response.data))
        })
            .catch(error => console.log(error.message))
    }

    const deleteTemplate = (id) =>{
        TemplateService.deleteTemplate(id)
            .then(response => {
                console.log(response.data)
                dispatch(removeTemplate(response.data))})
            .catch(error => console.log(error.message))

    }

    useEffect(() => {
        //dispatch(getAllTemplates(1))
        TemplateService.getAllTemplates()
            .then(response => {dispatch(getAllTemplates(response.data))})
            .catch(error => console.log(error.message))
    }, [])

    useEffect(() => {
        if (debouncedSearchTerm) {
            renderTemplate(mdMarkdown)
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
            <TemplateList
                templateList={templates}
                onDelete={id => deleteTemplate(id)}/>
        </div>
    );
};

export default Editor;