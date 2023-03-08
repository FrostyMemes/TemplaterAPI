import {configureStore} from "@reduxjs/toolkit";
import templateReducer from "./reducers/TemplateSlice"

export default configureStore({
    reducer: {
        templates: templateReducer
    },
})