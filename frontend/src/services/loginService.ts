import config from "../config";
import axios from "axios";
import {Credentials} from "../models/login/Credentials";
import {SignUpRequest} from "../models/login/SignUpRequest";

export function loginRequest(credentials: Credentials): Promise<any> {
	const form = new FormData();
	form.append('login', credentials.login);
	form.append('password', credentials.password);
	return axios.post(`${config.API_URL}/Users/Authorize`, form);
}

export function signUpRequest(request: SignUpRequest): Promise<any> {
	const form = new FormData();
	form.append('login', request.credentials.login);
	form.append('password', request.credentials.password);
	form.append('email', request.email);
	return axios.post(`${config.API_URL}/Users/Register`, form);
}
