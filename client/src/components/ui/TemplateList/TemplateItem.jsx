import React from 'react';
import api from "../../../http";

const TemplateItem = (props) => {

    return (
        <div className="template">
            <div className="template__content">
                <strong>{props.template.title}</strong>
                <div>
                    {props.template.id}
                </div>
            </div>
            <div>
                <input type="button" onClick={(e) => props.onDelete(props.template.id)} value="Удалить запись"/>
                <input type="button" value="Обновить запись"/>
            </div>
        </div>
    );
};

export default TemplateItem;