import React from 'react';

const EditorMarkdownArea = ({onChange, render}) => {

    return (
        <div className="editor-area">
            <div className="input-area">
                    <textarea placeholder={process.env.REACT_APP_BACKEND_ADDRESS}
                              onChange={(e) => onChange(e.target.value)}>
                    </textarea>
            </div>
            <div className="render-area" dangerouslySetInnerHTML={{__html: render}}></div>
        </div>
    )
}

export default EditorMarkdownArea