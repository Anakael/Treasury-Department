import {call, put, takeLatest} from 'redux-saga/effects';
import {loginRequest} from "../../../services/loginService";
import {logIn, logInFailure, logInSuccess} from "../actions/loginActions";

export function* login(logIn) {
	const credentials = logIn.payload;
	try {
		const response = yield call(loginRequest, credentials.login, credentials.password);
		const data = response.data;
		console.log(data);
		yield put(logInSuccess(data))
	} catch (error) {
		yield put(logInFailure(error.response.data))
	}
}

export function* watchLogin() {
	yield takeLatest(
		logIn,
		login,
	);
}
