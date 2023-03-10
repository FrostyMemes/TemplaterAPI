import React from 'react';
import classes from "./TemplateList.module.css";


const TemplateItem = ({template}) => {
    return (
        <div className={classes.templateItem}>
            <div className={classes.templateContent}>
                {template.title}
            </div>
        </div>
    );
};

export default TemplateItem;