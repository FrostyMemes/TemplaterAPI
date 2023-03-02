import React from 'react';
import api from "../http";

const TemplateItem = (props) => {

    function deleteTemplate () {
        api.delete(`${process.env.REACT_APP_BACKEND_TEMPLATE_ADDRESS}/${props.template.id}`)
            .then(response => console.log(response.data))
            .catch(error => console.log(error.message))
    }

    return (
        <div className="template">
            <div className="template__content">
                <strong>{props.template.title}</strong>
                <div>
                    {props.template.id}
                </div>
            </div>
            <div>
                <input type="button" onClick={deleteTemplate} value="Удалить запись"/>
                <input type="button" value="Обновить запись"/>
            </div>
        </div>
    );
};

export default TemplateItem;