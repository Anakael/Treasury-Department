import {createStore} from "redux";
import {rootReducer} from "./rootReducer";

export interface TreasuryDepartmentStore {
	auth: {
		token: string;
		authenticated: boolean;
		user?: string;
	}
}

export const initialStoreState = {};


export const store = createStore(rootReducer, initialStoreState);
