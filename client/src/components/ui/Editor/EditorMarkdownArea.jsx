import React from 'react';
import classes from "./Editor.module.css";

const EditorMarkdownArea = ({onChange}) => {

    return (

        <div className={classes.editorMarkdownArea}>
{/*            <textarea placeholder={process.env.REACT_APP_BACKEND_ADDRESS}
                      onChange={(e) => onChange(e.target.value)}>
            </textarea>*/}

        </div>

    )
}

export default EditorMarkdownArea