import {applyMiddleware, createStore} from "redux";
import {rootReducer} from "./rootReducer";
import {composeWithDevTools} from "redux-devtools-extension";
import createSagaMiddleware from 'redux-saga'
import {all, fork} from 'redux-saga/effects';
import {watchLogin} from "./login/sagas/loginSagas";

const composeEnhancers = composeWithDevTools({});
const sagaMiddleware = createSagaMiddleware();

export const store = createStore(rootReducer, {}, composeEnhancers(applyMiddleware(sagaMiddleware)));

const rootSaga = function* root() {
	yield all([fork(watchLogin)]);
};
sagaMiddleware.run(rootSaga);
