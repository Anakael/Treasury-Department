import {call, put, takeLatest} from 'redux-saga/effects';
import {loginRequest, signUpRequest} from "../../../services/loginService";
import {logIn, logInFailure, logInSuccess, signUp, signUpFailure} from "../actions/loginActions";

export function* login(logIn) {
	try {
		const response = yield call(loginRequest, logIn.payload);
		const data = response.data;
		yield put(logInSuccess(data))
	} catch (error) {
		yield put(logInFailure(error.response.data))
	}
}

export function* signup(signUp) {
	try {
		const response = yield call(signUpRequest, signUp.payload);
		const data = response.data;
		yield put(logInSuccess(data))
	} catch (error) {
		yield put(signUpFailure(error.response.data))
	}
}

export function* watchLogin() {
	yield takeLatest(
		logIn,
		login,
	);
	yield takeLatest(
		signUp,
		signup,
	);
}
