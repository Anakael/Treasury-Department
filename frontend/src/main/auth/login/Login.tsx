import React, {Component, ReactNode} from 'react';
import {Field, Form} from "react-final-form";
import {TextField} from 'final-form-material-ui';
import {Button} from "@material-ui/core";
import {connect, ConnectedProps} from "react-redux";
import {Dispatch} from "redux";
import {logIn} from "../../../store/login/actions/loginActions";
import {RootState} from "../../../store/rootReducer";
import {Redirect} from 'react-router-dom'

interface LoginState {
	login: string;
	password: string;
}

const mapStateToProps = (state: RootState) => ({
	auth: state.auth
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
	logIn: (login: string, password: string) => dispatch(logIn(login, password))
});

const connector = connect(
	mapStateToProps,
	mapDispatchToProps
);

type PropsFromRedux = ConnectedProps<typeof connector>

type LoginProps = PropsFromRedux

class LoginComponent extends Component<LoginProps, LoginState> {
	render(): ReactNode {
		return (
			this.props.auth.token !== ''
				? <Redirect to="/"/>
				: <div>
					<Form
						onSubmit={(formState: LoginState) => this.props.logIn(formState.login, formState.password)}
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
					<span>{this.props.auth.loginError}</span>
				</div>
		);
	}
}

export const Login = connector(LoginComponent);
