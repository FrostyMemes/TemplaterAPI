import React from 'react';

const TemplateItem = (props) => {
    return (
        <div className="template">
            <div className="template__content">
                <strong>{props.template.title}</strong>
                <div>
                    {props.template.id}
                </div>
            </div>
        </div>
    );
};

export default TemplateItem;