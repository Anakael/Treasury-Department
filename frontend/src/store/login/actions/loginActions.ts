import {LOGIN_FAILURE, LOGIN_REQUEST, LOGIN_SUCCESS} from "./actionTypes";
import {createAction} from "typesafe-actions";
import {Credentials} from "../../../models/login/Credentials";
import {LoginSuccessResult} from "../../../models/login/LoginSuccesResult";
import {SignUpRequest} from "../../../models/login/SignUpRequest";


export const logIn = createAction(LOGIN_REQUEST)<Credentials>();
export const logInSuccess = createAction(LOGIN_SUCCESS)<LoginSuccessResult>();
export const logInFailure = createAction(LOGIN_FAILURE)<string>();

export const signUp = createAction(LOGIN_REQUEST)<SignUpRequest>();
export const signUpSuccess = createAction(LOGIN_SUCCESS)<LoginSuccessResult>();
export const signUpFailure = createAction(LOGIN_FAILURE)<string>();
