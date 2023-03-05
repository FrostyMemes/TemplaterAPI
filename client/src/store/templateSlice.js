import {createSlice} from "@reduxjs/toolkit";
import * as TemplateService from "../services/TemplateService";

const templateSlice = createSlice({
    name: 'templates',
    initialState:{
        templates:[]
    },
    reducers:{
        addTemplate(state, action){
            state.templates.push(action.payload)
        },

        removeTemplate(state, action){
            state.templates = state.templates.filter(template => template.id !== action.payload.id)
        },

        getAllTemplates(state, action){
            state.templates = action.payload
        }
    },
})

export const {addTemplate, removeTemplate, getAllTemplates} = templateSlice.actions;

export default templateSlice.reducer;