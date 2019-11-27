import {LOGIN_FAILURE, LOGIN_REQUEST, LOGIN_SUCCESS, SIGNUP_FAILURE, SIGNUP_REQUEST} from "./actionTypes";
import {createAction} from "typesafe-actions";
import {Credentials} from "../../../models/login/Credentials";
import {LoginSuccessResult} from "../../../models/login/LoginSuccesResult";
import {SignUpRequest} from "../../../models/login/SignUpRequest";


export const logIn = createAction(LOGIN_REQUEST)<Credentials>();
export const logInSuccess = createAction(LOGIN_SUCCESS)<LoginSuccessResult>();
export const logInFailure = createAction(LOGIN_FAILURE)<string>();

export const signUp = createAction(SIGNUP_REQUEST)<SignUpRequest>();
export const signUpFailure = createAction(SIGNUP_FAILURE)<string>();
