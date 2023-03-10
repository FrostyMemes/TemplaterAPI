import React from 'react';
import TemplateItem from "./TemplateItem";
import classes from "./TemplateList.module.css"

const TemplateList = (props) => {
    return (
        <div className={classes.templateList}>
            {props.templateList.map(template =>
                <TemplateItem template={template} key={template.id}/>
            )}
        </div>
    );
}

export default TemplateList;