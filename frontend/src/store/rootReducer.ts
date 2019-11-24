import {combineReducers} from "redux";
import {loginReducer} from "./login/reducers/loginReducer";

export const rootReducer = combineReducers(
	{
		auth: loginReducer,
	}
);

export type RootState = ReturnType<typeof rootReducer>
