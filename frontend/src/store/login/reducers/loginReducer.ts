import {ActionType, createReducer} from "typesafe-actions";
import {User} from "../../../models/user";
import * as loginActions from "../actions/loginActions";
import {LOGIN_REQUEST} from "../actions/actionTypes";


export type LoginState = Readonly<{
	user: User;
	token: string;
}>;

const initialState: LoginState = {
	user: {},
	token: '',
};

export type LoginAction = ActionType<typeof loginActions>;

export const loginReducer = createReducer(initialState)
	.handleAction(LOGIN_REQUEST, (state: LoginState, action: LoginAction) => {
		console.log('LOL');
		return state;
	} );
