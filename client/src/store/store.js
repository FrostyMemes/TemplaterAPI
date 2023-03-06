import {configureStore} from "@reduxjs/toolkit";
import templateReducer from "./reducers/templateSlice"

export default configureStore({
    reducer: {
        templates: templateReducer
    },
})