import {takeEvery} from 'redux-saga/effects';
import {LOGIN_REQUEST} from "../actions/actionTypes";

export function* login() {
	console.log('LOGIN SIDE');
	yield ''
}

export function* watchLogin() {
	yield takeEvery(
		LOGIN_REQUEST,
		login
	);
}
