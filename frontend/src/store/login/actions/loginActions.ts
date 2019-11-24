import {LOGIN_REQUEST} from "./actionTypes";
import {createAction} from "typesafe-actions";

export const logIn = createAction(LOGIN_REQUEST)<string, string>();


