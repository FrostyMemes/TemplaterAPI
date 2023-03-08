import React from 'react';

const EditorMarkdownArea = ({onChange}) => {

    return (

        <div className="input-area">
            <textarea placeholder={process.env.REACT_APP_BACKEND_ADDRESS}
                      onChange={(e) => onChange(e.target.value)}>
            </textarea>
        </div>

    )
}

export default EditorMarkdownArea