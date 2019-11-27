import {Credentials} from "./Credentials";

export interface SignUpRequest {
	credentials: Credentials;
	email: string;
}
