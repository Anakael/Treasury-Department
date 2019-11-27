import {Field, Form, FormikProps} from "formik";
import TextField from "@material-ui/core/TextField";
import {Button} from "@material-ui/core";
import React from "react";
import {Credentials} from "../../../models/login/Credentials";
import {AuthState} from "../../../store/login/reducers/loginReducer";

export interface SignUpValues {
	credentials: Credentials
	confirmPassword: string;
	email: string;
}

interface OtherProps {
	auth: AuthState
}

export const InnerForm = (props: FormikProps<SignUpValues> & OtherProps) => {
	const {errors, auth} = props;
	return (
		<Form>
			<div>
				<Field name="credentials.login">
					{({field}) => (
						<div>
							<TextField {...field} type="text" placeholder="Login" required/>
						</div>
					)}
				</Field>
			</div>
			<div>
				<Field name="credentials.password">
					{({field}) => (
						<div>
							<TextField {...field} type="password" placeholder="Password" required/>
						</div>
					)}
				</Field>
				<Field name="confirmPassword">
					{({field}) => (
						<div>
							<TextField {...field} type="password" placeholder="Confirm password" required/>
							{errors.confirmPassword && <span>{errors.confirmPassword}</span>}
						</div>
					)}
				</Field>
				<Field name="email">
					{({field}) => (
						<div>
							<TextField {...field} type="email" placeholder="Email" required/>
						</div>
					)}
				</Field>
			</div>
			<div>
				<Button type="submit">
					Sign up
				</Button>
				{auth.signUpError && <span>{auth.signUpError}</span>}
			</div>
		</Form>
	)
};
