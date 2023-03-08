import * as TemplateService from "../../services/TemplateService";
import {createAsyncThunk} from "@reduxjs/toolkit";

export const fetchTemplates = createAsyncThunk(
    'templates/fetchTemplates',
    async (_, thunkAPI) => {
        try {
            const response = await TemplateService.getAllTemplates()
            return response.data
        } catch (e){
            return thunkAPI.rejectWithValue(e)
        }

    }
)

export const saveTemplate = createAsyncThunk(
    'templates/saveTemplate',
    async (template, thunkAPI) => {
        try {
            const response = await TemplateService.saveTemplate(template)
            return response.data
        } catch (e){
            return thunkAPI.rejectWithValue(e)
        }

    }
)

export const deleteTemplate = createAsyncThunk(
    'templates/deleteTemplate',
    async (id, thunkAPI) => {
        try {
            const response = await TemplateService.deleteTemplate(id)
            return response.data
        } catch (e){
            return thunkAPI.rejectWithValue(e)
        }

    }
)