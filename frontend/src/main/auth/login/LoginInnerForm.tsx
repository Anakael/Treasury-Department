import {Credentials} from "../../../models/login/Credentials";
import {Field, Form, FormikProps} from "formik";
import TextField from "@material-ui/core/TextField";
import {Button} from "@material-ui/core";
import React from "react";
import {AuthState} from "../../../store/login/reducers/loginReducer";

interface OtherProps {
	auth: AuthState
}

export const InnerForm = (props: FormikProps<Credentials> & OtherProps) => {
	const {auth} = props;
	return (
		<Form>
			<div>
				<Field name="login">
					{({field}) => (
						<div>
							<TextField {...field} type="text" placeholder="Login" required/>
						</div>
					)}
				</Field>
			</div>
			<div>
				<Field name="password">
					{({field}) => (
						<div>
							<TextField {...field} type="password" placeholder="Password" required/>
						</div>
					)}
				</Field>
			</div>
			<div>
				<Button type="submit">
					Log in
				</Button>
				{auth.loginError && <span>{auth.loginError}</span>}
			</div>
		</Form>
	);
};
