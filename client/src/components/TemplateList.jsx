import React from 'react';
import TemplateItem from "./TemplateItem";

const TemplateList = (props) => {
    return (
        <div className="template__list">
            {props.templateList.map(template =>
                <TemplateItem template={template} key={template.id}/>
            )}
        </div>
    );
}

export default TemplateList;