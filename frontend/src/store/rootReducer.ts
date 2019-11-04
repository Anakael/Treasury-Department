import {combineReducers} from "redux";
import {loginReducer} from "./login/reducers/loginReducer";

export const rootReducer = combineReducers(
	{
		login: loginReducer,
	}
);
