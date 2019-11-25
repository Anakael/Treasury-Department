import config from "../config";
import axios from "axios";

export function loginRequest(login: string, password: string): Promise<any> {
	const form = new FormData();
	form.append('login', login);
	form.append('password', password);
	return axios.post(`${config.API_URL}/Users/Authorize`, form);
}
