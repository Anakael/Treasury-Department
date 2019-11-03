import React, {Component, ReactNode} from 'react';
import {Field, Form} from "react-final-form";
import {TextField} from 'final-form-material-ui';
import {Button} from "@material-ui/core";

interface LoginState {
	login: string;
	password: string;
}

export class Login extends Component<{}, LoginState> {
	render(): ReactNode {
		return (
			<div>
				<Form
					onSubmit={(values) => console.log(values)}
					initialValues={{login: '', password: ''}}
					render={({handleSubmit, form, submitting, pristine, values}) => (
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
