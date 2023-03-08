import React from 'react';

const EditorRenderArea = ({render}) => {
    return (
        <div className="render-area" dangerouslySetInnerHTML={{__html: render}}></div>
    );
};

export default EditorRenderArea;