import {User} from "./User";
import {Token} from "./Token";

export interface LoginSuccessResult {
	user: User,
	token: Token
}
