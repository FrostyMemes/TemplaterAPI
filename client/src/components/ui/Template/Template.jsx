import React, {useEffect, useState} from 'react';
import {useSelector} from "react-redux";
import classes from "./Template.module.css";

const Template = () => {

    const template = useSelector(state => state.templates.currentTemplate)

    return (
        <div className={classes.template}>
            <div dangerouslySetInnerHTML={{__html: template?.markup}}></div>
        </div>
    );
};

export default Template;