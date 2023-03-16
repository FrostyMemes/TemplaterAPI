import React from 'react';
import {useEffect, useState} from "react";
import {useSelector, useDispatch} from "react-redux";
import useDebounce from "../../hooks/use-debounce";
import EditorMarkdownArea from "./EditorMarkdownArea";
import TemplateList from "../TemplateList/TemplateList";
import {sendMarkdown} from "../../../services/MarkdownService";
import {fetchTemplates, saveTemplate, deleteTemplate} from "../../../store/reducers/TemplateActions";
import EditorRenderArea from "./EditorRenderArea";
import classes from "./Editor.module.css";


const Editor = () => {

    // const templates = useSelector(state => state.templates.templates)
    // const dispatch = useDispatch()

    const [mdMarkdown, setMarkdown] = useState('')
    const [mdRender, setRender] = useState('')

    const debouncedSearchTerm = useDebounce(mdMarkdown, 700);

    const renderTemplate = (markdown) => {
        sendMarkdown(markdown)
            .then(response => setRender(response.data))
            .catch(error => setRender(error.message))
    }

    // const addTemplate = () => {
    //     dispatch(saveTemplate({
    //             title: mdTitle,
    //             markdown: mdMarkdown,
    //             markup: mdRender
    //         })
    //     )
    // }
    //
    // const removeTemplate = (id) => {
    //     dispatch(deleteTemplate(id))
    // }
    //
    // useEffect(() => {
    //     dispatch(fetchTemplates())
    // }, [])

    useEffect(() => {
        if (debouncedSearchTerm) {
            renderTemplate(mdMarkdown)
        } else {
            setRender('');
        }
    }, [debouncedSearchTerm])

    return (
            <div className={classes.editorArea}>
                <EditorMarkdownArea onChange={markdown => setMarkdown(markdown)}/>
                <EditorRenderArea render={mdRender}/>
            </div>
    );
};

export default Editor;