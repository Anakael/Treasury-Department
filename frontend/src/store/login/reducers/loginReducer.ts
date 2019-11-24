import {ActionType} from "typesafe-actions";
import {User} from "../../../models/user";
import * as loginActions from "../actions/loginActions";
import {LOGIN_REQUEST} from "../actions/actionTypes";
import Cookies from 'js-cookie'


export type AuthState = Readonly<{
	user: User;
	token: string;
	loginError: string;
}>;

export const authInitialState: AuthState = {
	user: {},
	token: Cookies.get('token') || '',
	loginError: ''
};

export type LoginAction = ActionType<typeof loginActions>;

export function loginReducer(
	state = authInitialState,
	action: LoginAction
): AuthState {
	switch (action.type) {
		case LOGIN_REQUEST: {
			console.log(Cookies.get('token'));
			const token = 'lol';
			Cookies.set('token', token);
			return {
				...state,
				token: token,
			};
		}
		default:
			return state
	}
}
