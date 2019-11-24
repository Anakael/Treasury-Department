import {createStore} from "redux";
import {rootReducer} from "./rootReducer";
import {devToolsEnhancer} from "redux-devtools-extension";


export const store = createStore(rootReducer, devToolsEnhancer({}));
