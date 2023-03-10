import './App.css';
import React, {useState, useEffect} from "react";
import {useDispatch, useSelector} from "react-redux";
import TemplateList from "./components/ui/TemplateList/TemplateList";
import {fetchTemplates} from "./store/reducers/TemplateActions";
import Editor from "./components/ui/Editor/Editor";
import Template from "./components/ui/Template/Template";

function App() {
    // const templates = useSelector(state => state.templates.templates)
    // const dispatch = useDispatch()
    //
    // useEffect(() => {
    //     dispatch(fetchTemplates())
    // }, [])

    const [templates, setTemplates] = useState([
        {id: 1, title: "Template1"},
        {id: 2, title: "Template2"},
        {id: 3, title: "Template3"},
    ])

    return (
        <div className="workspace">
            <TemplateList templateList={templates}/>
            <Template/>
        </div>
    );
}

export default App;
