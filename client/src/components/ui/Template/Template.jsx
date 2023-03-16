import React, {useEffect, useState} from 'react';
import {useSelector} from "react-redux";
import  "./Template.scss";


const Template = () => {

    const template = useSelector(state => state.templates.currentTemplate)

    return (
        <div className="template-block">
                <div dangerouslySetInnerHTML={{__html: template?.markup}}></div>
        </div>
    );
};

export default Template;