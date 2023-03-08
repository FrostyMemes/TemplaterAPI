import {createSlice} from "@reduxjs/toolkit";
import {
    fetchTemplates,
    deleteTemplate,
    saveTemplate
} from "./TemplateActions";

const templateSlice = createSlice({
    name: 'templates',
    initialState:{
        templates:[],
        isLoading: false,
        error: null
    },

    reducers:{

    },

    extraReducers:{
        [fetchTemplates.pending]: (state, action) => {
            state.status = true
        },
        [fetchTemplates.fulfilled]: (state, action) => {
            state.templates = action.payload
            state.status = false
        },
        [fetchTemplates.rejected]: (state, action) => {
            state.status = false
            state.error = action.payload
        },

        [saveTemplate.pending]:(state, action) => {

        },
        [saveTemplate.fulfilled]:(state, action) => {
            console.log(action.payload)
            state.templates.push(action.payload)
        },
        [saveTemplate.rejected]:(state, action) => {
            state.error = action.payload
        },

        [deleteTemplate.pending]:(state, action) => {

        },
        [deleteTemplate.fulfilled]:(state, action) => {
            console.log(action.payload)
            state.templates = state.templates.filter(template => template.id !== action.payload.id)
        },
        [deleteTemplate.rejected]:(state, action) => {
            state.error = action.payload
        },
    },
})

export const {addTemplate, removeTemplate, getAllTemplates} = templateSlice.actions;

export default templateSlice.reducer;