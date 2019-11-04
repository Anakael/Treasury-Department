import React, {Component, ReactNode} from 'react';
import {Field, Form, FormRenderProps} from "react-final-form";
import {TextField} from 'final-form-material-ui';
import {Button} from "@material-ui/core";
import {connect} from "react-redux";
import {store} from "../../../store/treasuryDepartmentStore";
import {LOGIN_REQUEST} from "../../../store/login/actions/actionTypes";

interface LoginState {
	login: string;
	password: string;
}

class LoginComponent extends Component<{}, LoginState> {
	render(): ReactNode {
		return (
			<div>
				<Form
					onSubmit={(values: FormRenderProps<LoginState>) => store.dispatch({ type: LOGIN_REQUEST})}
					render={({handleSubmit}) => (
						<form onSubmit={handleSubmit}>
							<div>
								<Field
									name="login"
									type="text"
									component={TextField}
									label="Login"
								/>
							</div>
							<div>
								<Field
									name="password"
									type="password"
									component={TextField}
									label="Password"
								/>
							</div>
							<Button type="submit">Log in</Button>
						</form>
					)}
				/>
			</div>
		);
	}
}

const mapStateToProps = (state: LoginState) => ({
});

const dispatchProps = {

};

export const Login = connect(
	mapStateToProps,
	dispatchProps
)(LoginComponent);
