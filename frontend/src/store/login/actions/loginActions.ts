import {LOGIN_FAILURE, LOGIN_REQUEST, LOGIN_SUCCESS} from "./actionTypes";
import {createAction} from "typesafe-actions";
import {Credentials} from "../../../models/login/Credentials";
import {LoginSuccessResult} from "../../../models/login/LoginSuccesResult";


export const logIn = createAction(LOGIN_REQUEST)<Credentials>();
export const logInSuccess = createAction(LOGIN_SUCCESS)<LoginSuccessResult>();
export const logInFailure = createAction(LOGIN_FAILURE)<string>();


