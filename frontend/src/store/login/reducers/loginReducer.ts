import {ActionType} from "typesafe-actions";
import {User} from "../../../models/login/User";
import * as loginActions from "../actions/loginActions";
import {LOGIN_FAILURE, LOGIN_SUCCESS, SIGNUP_FAILURE} from "../actions/actionTypes";
import Cookies from 'js-cookie'
import {Token} from "../../../models/login/Token";


export type AuthState = Readonly<{
	user: User;
	token: Token;
	loginError: string;
	signUpError: string;
}>;

export const authInitialState: AuthState = {
	user: (Cookies.get('user') && JSON.parse(Cookies.get('user'))) || '',
	token: (Cookies.get('token') && JSON.parse(Cookies.get('token'))) || '',
	loginError: '',
	signUpError: ''
};

export type LoginAction = ActionType<typeof loginActions>;

export function loginReducer(
	state = authInitialState,
	action: LoginAction
): AuthState {
	switch (action.type) {
		case LOGIN_SUCCESS: {
			const payload = action.payload;
			const token = payload.token;
			const user = payload.user;
			Cookies.set('user', user);
			Cookies.set('token', token);
			return {
				...state,
				loginError: '',
				signUpError: '',
				user: user,
				token: token,
			};
		}
		case LOGIN_FAILURE: {
			Cookies.remove('user');
			Cookies.remove('token');
			return {
				...state,
				user: authInitialState.user,
				token: authInitialState.token,
				loginError: action.payload,
			};
		}
		case SIGNUP_FAILURE: {
			Cookies.remove('user');
			Cookies.remove('token');
			return {
				...state,
				user: authInitialState.user,
				token: authInitialState.token,
				signUpError: action.payload,
			};
		}
		default:
			return state
	}
}
