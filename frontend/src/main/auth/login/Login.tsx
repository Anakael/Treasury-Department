import React, {FC} from 'react';
import {Button} from "@material-ui/core";
import {connect, ConnectedProps} from "react-redux";
import {RootState} from "../../../store/rootReducer";
import {Link, Redirect} from 'react-router-dom'
import LoginForm from "./LoginForm";


const mapStateToProps = (state: RootState) => ({
	auth: state.auth
});

const connector = connect(
	mapStateToProps,
	{}
);

type PropsFromRedux = ConnectedProps<typeof connector>

type LoginProps = PropsFromRedux

export const Login: FC<LoginProps> = (props) => {
	return (
		!!props.auth.token
			? <Redirect to="/"/>
			: <div>
				<LoginForm/>
				<Link to={'/signup'}> <Button>Sign up</Button></Link>
			</div>
	);
};

export default connector(Login);
